/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Read;
using Read.Management;

namespace Web.Features.Accounts
{
    /// <summary>
    /// 
    /// </summary>
    [SecurityHeaders]
    [Route("Authorities")]
    public class AuthoritiesController : Controller
    {
        readonly IAuthenticationSchemeProvider _schemeProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemeProvider"></param>
        /// <param name="tenant"></param>
        public AuthoritiesController(IAuthenticationSchemeProvider schemeProvider, Tenant tenant)
        {
            _schemeProvider = schemeProvider;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(_ =>
                    !string.IsNullOrEmpty(_.DisplayName)
                )
                .Select(_ => new Authority
                {
                    Id = Guid.Parse(_.Name),

                        // Overridable name
                        Name = _.DisplayName,
                        AuthenticationScheme = _.Name
                });

            // Allow remember login?
            return Json(providers);
        }
    }
}