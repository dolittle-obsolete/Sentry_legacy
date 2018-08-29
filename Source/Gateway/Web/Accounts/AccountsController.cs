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
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Accounts
{

    /// <summary>
    /// 
    /// </summary>
    [SecurityHeaders]
    [Route("Accounts")]
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

        // http://localhost:5000/Accounts/ExternalLogin?authority=9b296977-7657-4bc8-b5b0-3f0a23c43958&returnUrl=%2Fbe4c4da6-5ede-405f-a947-8aedad564b7f%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3Dmvc%26redirect_uri%3Dhttp%253A%252F%252Flocalhost%253A5002%252Fsignin-oidc%26response_type%3Did_token%26scope%3Dopenid%2520profile%2520nationalsociety%26response_mode%3Dform_post%26nonce%3D636579958176163830.ZTg1MWNmNGQtZTBlNy00NDE5LTkzZDItYTJlODcxNjkxZWYwZDdjZDVmMmItNDBlMS00NzUzLWI1NTYtODdmYzkxYTE1ODMw%26state%3DCfDJ8CeXhV5JBLFMkMDAcLC1jYLc48KUu79-SKX6z2gz23l0pg4qU2yygyzK31XVzYqTPim93p05ika5P_t0PARdso-QzuUOTDX8iOLgSwv0dyNrPA02oGundKe6-6SKfRuuZXxq5nQbbQIB2Q8BBrPQLfk67LeGird0dk-W3Opc5idbzHc1ATf7Qq_HpsAMoYcj1NwU44_8O7oKo-PMN5LW63IXrkkhqJihySqY7Kr5QN12AeUhppA3fp58xTBOWbK-zW7SVHkReShBV7dnRDrp-Z2yGPlJ_pEIEDjqgBcP4MNdyWiQLJlyPNyM7pNyZCte73EhU4EoOjrBNL9cEo7nizY%26x-client-SKU%3DID_NET%26x-client-ver%3D2.1.4.0

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
            if( hasTenant )
            {
                var tenantConfiguration = _tenants.GetFor(tenantId);
                if( tenantConfiguration != null )
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