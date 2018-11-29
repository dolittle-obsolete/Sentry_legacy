/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using Concepts.Authorities;
using Dolittle.ReadModels;

namespace Read.Studio.Authority
{
    /// <summary>
    /// Represents an authority
    /// </summary>
    public class Authority : IReadModel
    {
        /// <summary>
        /// Gets or sets the <see cref="AuthorityId"/>
        /// </summary>
        public AuthorityId Id { get; set; }

        /// <summary>
        /// Gets or sets the url for the authority
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the client id used towards the authority
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the authority
        /// </summary>
        public string Name {  get; set; }

        /// <summary>
        /// Gets or sets the description of the authority
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the authentication scheme
        /// </summary>
        public string AuthenticationScheme {  get; set; }

        /// <summary>
        /// Gets or sets the url of the icon
        /// </summary>
        public string IconUrl {  get; set; }
    }
}