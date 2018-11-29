/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Read.Authorities
{
    /// <summary>
    /// Represents a <see cref="ExternalAuthority"/> with specifics for OpenId Connect scenarios
    /// </summary>
    public class OpenIdConnectExternalAuthority : ExternalAuthority
    {
        /// <summary>
        /// Gets or sets the client id that the authority will recognize
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Uri for the authority
        /// </summary>
        public string Authority { get; set; }
    }   
}