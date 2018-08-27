/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Events;

namespace Events.Accounts
{
    /// <summary>
    /// Represents the <see cref="IEvent"/> that occurs when a user has granted access to a specific application
    /// </summary>
    public class ApplicationAccessGrantedByUser : IEvent
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationAccessGrantedByUser"/>
        /// </summary>
        /// <param name="user">User that is granting access</param>
        /// <param name="application">Application that has been granted access</param>
        public ApplicationAccessGrantedByUser(Guid user, Guid application)
        {
            User = user;
            Application = application;
        }

        /// <summary>
        /// Gets the unique identifier of the user
        /// </summary>
        public Guid User { get; }


        /// <summary>
        /// Gets the unique identifier of the application
        /// </summary>
        public Guid Application { get; }
    }
}