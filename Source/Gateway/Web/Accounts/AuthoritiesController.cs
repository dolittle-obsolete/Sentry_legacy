/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Web.Accounts
{
    /// <summary>
    /// 
    /// </summary>
    [SecurityHeaders]
    [Route("Authorities")]
    public class AuthoritiesController : Controller
    {
        readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IAuthContext _authContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemeProvider"></param>
        /// <param name="authContext"></param>
        public AuthoritiesController(IAuthenticationSchemeProvider schemeProvider, IAuthContext authContext)
        {
            _schemeProvider = schemeProvider;
            _authContext = authContext;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();

            // Todo: Allow remember login?

            var providers = _authContext
                                .Application
                                    .ExternalAuthorities
                                        .Where(
                                            authority => 
                                                schemes.Any(scheme => scheme.Name == authority.Type.ToString()
                                            )
                                        );
            return Json(providers);
        }
    }
}