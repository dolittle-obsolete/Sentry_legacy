/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Concepts.Resources;
using Dolittle.Commands;

namespace Domain.Studio.Resources
{
    /// <summary>
    /// Represents an intent to enable an identity resource
    /// </summary>
    public class EnableIdentityResource : ICommand
    {
        /// <summary>
        /// Gets or sets the name of the identity resource
        /// </summary>
        public ResourceName Name { get; set; }
    }
}