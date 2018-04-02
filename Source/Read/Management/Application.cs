/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Read.Management
{
    /// <summary>
    /// Represents the configuration of an application
    /// </summary>
    public class Application 
    {
        /// <summary>
        /// Gets or sets the <see cref="ResourceDefinition">resource definitions</see>
        /// </summary>
        public IEnumerable<ResourceDefinition> ResourceDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the Api resources available to be granted access to for the application
        /// </summary>
        public IEnumerable<string> ApiResources { get; set; }

        /// <summary>
        /// Gets or sets the Identity resources available to be granted access to for the application
        /// </summary>
        public IEnumerable<string> IdentityResources { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Client">clients</see> of the application
        /// </summary>
        public IEnumerable<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ProfileClaim">profile claims</see> for the application
        /// </summary>
        public IEnumerable<ProfileClaim> ProfileClaims { get; set; }

        /// <summary>
        /// Gets or sets the allowed <see cref="ExternalAuthority">external authorities</see>
        /// </summary>
        public IEnumerable<ExternalAuthority> ExternalAuthorities { get; set; }
    }
}