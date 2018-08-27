/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Events;

namespace Events.Accounts
{
    /// <summary>
    /// Represents the <see cref="IEvent"/> that occurs when a claim is added to a user
    /// </summary>
    public class ClaimAddedToUser : IEvent
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ClaimAddedToUser"/>
        /// </summary>
        /// <param name="user">User to add claim to</param>
        /// <param name="claim">Name of claim to add</param>
        /// <param name="value">Value of the claim to add</param>
        public ClaimAddedToUser(Guid user, string claim, string value)
        {
            User = user;
            Claim = claim;
            Value = value;
        }

        /// <summary>
        /// Gets the unique identifier representing the user
        /// </summary>
        public Guid User { get; }

        /// <summary>
        /// Gets the name of the claim
        /// </summary>
        public string Claim { get; }

        /// <summary>
        /// Gets the value of claim
        /// </summary>
        public string Value { get; }
    }
}