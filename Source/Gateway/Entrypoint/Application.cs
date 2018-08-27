/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Entrypoint
{
    /// <summary>
    /// Represents the configuration of an application
    /// </summary>
    public class Application 
    {
        /// <summary>
        /// Gets or sets the <see cref="Resource">Api resources</see> available to be granted access to for the application
        /// </summary>
        public IEnumerable<Resource> ApiResources { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Resource">identity resources</see> available to be granted access to for the application
        /// </summary>
        public IEnumerable<Resource> IdentityResources { get; set; }

        /// <summary>
        /// Gets or sets the well known identity resources by name
        /// </summary>
        public IEnumerable<string> WellKnownIdentityResources { get; set; }

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
        public IEnumerable<AzureActiveDirectoryExternalAuthority> ExternalAuthorities { get; set; }
    }
}