/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Concepts;
using Concepts.Claims;
using Dolittle.Commands;

namespace Domain.Studio.UserManagement
{
    /// <summary>
    /// The <see cref="ICommand"/> that represents adding 
    /// </summary>
    public class AddClaimToUser : ICommand
    {
        /// <summary>
        /// Gets or sets the <see cref="UserId">user</see> that should get the claim
        /// </summary>
        public UserId User { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ClaimName">name of the claim</see>
        /// </summary>
        public ClaimName ClaimName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ClaimValue">value for the claim</see> to add
        /// </summary>
        public ClaimValue ClaimValue { get; set; }
    }
}