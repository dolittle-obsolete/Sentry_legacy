/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Read.Tenants;

namespace Core.Accounts
{

    /// <summary>
    /// 
    /// </summary>
    [SecurityHeaders]
    [Route("api/Accounts")]
    public class AccountsController : Controller
    {
        readonly IIdentityServerInteractionService _interaction;
        readonly IAuthenticationSchemeProvider _schemeProvider;
        readonly IEventService _events;
        readonly ITenantConfiguration _tenants;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interaction"></param>
        /// <param name="schemeProvider"></param>
        /// <param name="events"></param>
        /// <param name="tenants"></param>
        public AccountsController(
            IIdentityServerInteractionService interaction,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            ITenantConfiguration tenants)
        {
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _events = events;
            _tenants = tenants;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="authority"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet("ExternalLogin")]
        public IActionResult ExternalLogin(
            [FromQuery] string tenant, 
            [FromQuery] string authority, 
            [FromQuery] string returnUrl)
        {
            var tenantName = string.Empty;
            Guid tenantId = Guid.Empty;
            var hasTenant = Guid.TryParse(tenant, out tenantId);
            if ( hasTenant )
            {
                var tenantConfiguration = _tenants.GetFor(tenantId);
                if ( tenantConfiguration != null )
                {
                    tenantName = tenantConfiguration.Name;
                }
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback"),

                Items = 
                { 
                    { "tenant", tenant },
                    { "tenantName", tenantName },
                    { "scheme", authority },
                    { "returnUrl", returnUrl },
                    
                }
            };
            return Challenge(properties, authority);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            var externalUser = result.Principal;
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");
            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;
            var subjectId = providerUserId;
            var username = externalUser.FindFirst(JwtClaimTypes.Name)?.Value ?? "";

            var claims = externalUser.Claims.ToList();
            var localSignInProps = new AuthenticationProperties();

            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = result.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                claims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var id_token = result.Properties.GetTokenValue("id_token");
            if (id_token != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
            }
            
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, subjectId, username));
            await HttpContext.SignInAsync(subjectId, username, provider, localSignInProps, claims.ToArray());

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // validate return URL and redirect back to authorization endpoint or a local page
            var returnUrl = result.Properties.Items["returnUrl"];
            if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
        }
    }
}