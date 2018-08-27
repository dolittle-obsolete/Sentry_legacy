/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Concepts;
using Concepts.Resources;
using Dolittle.Commands;

namespace Domain.Management.Resources
{
    /// <summary>
    /// Represents an intent to emphasize an identity resource
    /// </summary>
    /// <remarks>
    /// By emphasizing an identity resource, it will be shown with an emphazis on the consent screen
    /// </remarks>
    public class EmphasizeIdentityResource : ICommand
    {
        /// <summary>
        /// Gets or sets the name of the identity resource
        /// </summary>
        public Name Name {  get; set; }
    }
}