/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dolittle.Collections;
using Dolittle.Logging;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.AuthenticationMiddleware
{

    public class TokenRequestValidator : ICustomTokenRequestValidator
    {
        public Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Route("auth")]
    public class Auth : Controller
    {
        private readonly ILogger _logger;

        public Auth(ILogger logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult> Index()
        {
            _logger.Information($"Request : {HttpContext.Request.Path.ToString()}");
            var unsecuredPaths = new []
            {
                "signin-oidc",
                ".well-known",
                "auth/signin",
                "connect",
                "device",
                "sentry"
            };
            if (unsecuredPaths.Any(_ => HttpContext.Request.Path.ToString().StartsWith(_))) return Ok();
            if (unsecuredPaths.Any(_ => HttpContext.Request.Path.ToString().StartsWith("/" + _))) return Ok();

            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-Uri"))
            {
                var forwardedUri = HttpContext.Request.Headers.FirstOrDefault(_ => _.Key == "X-Forwarded-Uri");
                if (unsecuredPaths.Any(_ => forwardedUri.Value.First().StartsWith("" + _))) return Ok();
                if (unsecuredPaths.Any(_ => forwardedUri.Value.First().StartsWith("/" + _))) return Ok();
            }

            _logger.Information("Run Auth on the request");
            HttpContext.Request.Headers.ForEach(_ => _logger.Information($"Header, {_.Key} : {_.Value}"));

            var result = await HttpContext.AuthenticateAsync("Dolittle");
            if (result.Succeeded)
            {
                HttpContext.AddTenant(result);
                HttpContext.AddCorrellation();
                HttpContext.AddMicroservice();
                return Ok();

            }

            result = await HttpContext.AuthenticateAsync("Bearer");
            if (result.Succeeded)
            {
                HttpContext.AddTenant(result);
                HttpContext.AddCorrellation();
                HttpContext.AddMicroservice();
                return Ok();
            }

            // select tenant -> claim
            // tenant http header (Auth)

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/sentry/accounts/tenantselector"
            }, "AzureAD");
        }

        [HttpGet("signin")]
        public async Task<IActionResult> SignIn([FromQuery] string tenant)
        {

            var result = await HttpContext.AuthenticateAsync("AzureAD");
            if (result.Succeeded)
            {
                // Verify that user has access to tenant
                await HttpContext.SignInAsync("Dolittle", new ClaimsPrincipal(new []
                {
                    new ClaimsIdentity(new []
                    {
                        new Claim(JwtClaimTypes.Subject, "kåre"),
                            new Claim(JwtClaimTypes.AuthenticationTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                            new Claim(JwtClaimTypes.IdentityProvider, "idsrv"),
                            new Claim("tid", tenant)
                    })
                }));

                await HttpContext.SignOutAsync("AzureAD");
                HttpContext.AddTenant(result);
                HttpContext.AddCorrellation();
                HttpContext.AddMicroservice();
            }

            return Redirect("/");
        }
    }
}