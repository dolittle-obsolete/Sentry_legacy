/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Commands.Coordination;
using Dolittle.Runtime.Commands;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Read.Consents;

namespace Web.Accounts
{

    /// <summary>
    /// 
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    [Route("Consent")]
    public class ConsentController : Controller
    {
        readonly IIdentityServerInteractionService _interaction;
        readonly IClientStore _clientStore;
        readonly IResourceStore _resourceStore;
        private readonly ICommandCoordinator _commandCoordinator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interaction"></param>
        /// <param name="clientStore"></param>
        /// <param name="resourceStore"></param>
        /// <param name="commandCoordinator"></param>
        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            ICommandCoordinator commandCoordinator)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _commandCoordinator = commandCoordinator;
        }

        //TODO: This is a command
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scopes"></param>
        /// <param name="returnUrl"></param>
        /// <param name="rememberConsent"></param>
        /// <returns></returns>
        [HttpPost("Grant")]
        public async Task<IActionResult> Grant(
            [FromForm]IEnumerable<string> scopes,
            [FromForm]string returnUrl,
            [FromForm]bool rememberConsent
        )
        {
            var grantedConsent = new ConsentResponse
            {
                RememberConsent = rememberConsent,
                ScopesConsented = scopes
            };
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _interaction.GrantConsentAsync(request, grantedConsent);
            
            return Redirect(returnUrl);
        }

        /*

        [HttpPost("Deny")]
        public async Task<IActionResult> Deny()
        {

        }*/
        
    }
}