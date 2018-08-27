/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Concepts.Resources;
using Dolittle.Commands;

namespace Domain.Management.Resources
{
    /// <summary>
    /// Represents the intent to add an identity resource in the context of the current tenant
    /// </summary>
    public class AddIdentityResource : ICommand
    {
        /// <summary>
        /// Gets or sets the name of the identity resource
        /// </summary>
        public Name Name {  get; set; }

        /// <summary>
        /// Gets or sets the displayname of the identity resource
        /// </summary>
        public DisplayName DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the displayname of the identity resource
        /// </summary>
        public Description Description {  get; set; }
    }
}