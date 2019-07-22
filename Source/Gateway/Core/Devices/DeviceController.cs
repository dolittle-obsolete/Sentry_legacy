/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Accounts;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Core.Devices
{
    /// <summary>
    /// 
    /// </summary>
    //[Authorize]
    [SecurityHeaders]
    [Route("device")]
    public class DeviceController : Controller
    {
        private readonly IDeviceFlowInteractionService _interaction;
        private readonly IDeviceFlowCodeService _devices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceFlowInteractionService"></param>
        /// <param name="devices"></param>
        public DeviceController(IDeviceFlowInteractionService deviceFlowInteractionService, IDeviceFlowCodeService devices)
        {
            _interaction = deviceFlowInteractionService;
            _devices = devices;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string userCode)
        {
            var result = await HttpContext.AuthenticateAsync("Dolittle");
            if (!result.Succeeded)
            {
                return Challenge(new AuthenticationProperties
                {
                    //RedirectUri = Url.Action(nameof(SignIn)),
                    RedirectUri = $"/sentry/accounts/tenantselector?userCode={userCode}",
                    Items = 
                    { 
                        { "userCode", userCode }
                    }
                }, "AzureAD");
            }

            var context = await _interaction.GetAuthorizationContextAsync(userCode);
            var consent = new ConsentResponse
            {
                ScopesConsented = context.ScopesRequested
            };

            var deviceAuth = await _devices.FindByUserCodeAsync(userCode);
            deviceAuth.IsAuthorized = true;
            deviceAuth.Subject = result.Principal;
            deviceAuth.AuthorizedScopes = consent.ScopesConsented;
            await _devices.UpdateByUserCodeAsync(userCode, deviceAuth);

            HttpContext.AddTenant(result);
            HttpContext.AddCorrellation();
            HttpContext.AddMicroservice();

            //var requestResult = await _interaction.HandleRequestAsync(userCode, consent);

            return Ok();
        }

        [HttpGet("signin")]
        public async Task<IActionResult> SignIn([FromQuery] string tenant)
        {
            var result = await HttpContext.AuthenticateAsync("AzureAD");
            if (result.Succeeded)
            {

                var principal = new ClaimsPrincipal(new []
                {
                    new ClaimsIdentity(new []
                    {
                        new Claim(JwtClaimTypes.Subject, "einarsen"),
                        new Claim(JwtClaimTypes.AuthenticationTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                        new Claim(JwtClaimTypes.IdentityProvider, "idsrv"),
                        new Claim("tid", tenant)
                    })
                });

                await HttpContext.SignInAsync("Dolittle", principal);
                await HttpContext.SignOutAsync("AzureAD");

                var userCode = result.Properties.Items.ContainsKey("userCode") ? result.Properties.Items["userCode"] : string.Empty;

                var context = await _interaction.GetAuthorizationContextAsync(userCode);
                var consent = new ConsentResponse
                {
                    ScopesConsented = context.ScopesRequested
                };

                var deviceAuth = await _devices.FindByUserCodeAsync(userCode);
                deviceAuth.IsAuthorized = true;
                deviceAuth.Subject = principal;
                deviceAuth.AuthorizedScopes = consent.ScopesConsented;
                await _devices.UpdateByUserCodeAsync(userCode, deviceAuth);


                HttpContext.AddTenant(result);
                HttpContext.AddCorrellation();
                HttpContext.AddMicroservice();

                //return Redirect($"/device?userCode={userCode}");
            }

            return Redirect("/");
        }

    }
}