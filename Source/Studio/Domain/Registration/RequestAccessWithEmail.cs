/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Concepts;
using Dolittle.Commands;
using Dolittle.Commands.Validation;

namespace Domain.Registration
{
    /// <summary>
    /// The <see cref="ICommand">command</see> that represents the intent of requesting access to
    /// an application.
    /// 
    /// Notification will happen through email
    /// </summary>
    public class RequestAccessWithEmail : ICommand
    {
        /// <summary>
        /// Gets or sets the <see cref="TenantId">tenant</see>
        /// </summary>
        public TenantId Tenant { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ApplicationName">name of the application</see>
        /// </summary>
        public ApplicationName Application { get; set; }

        /// <summary>
        /// Gets or sets the email used for notifying the user when access has been granted
        /// </summary>
        public string Email { get; set; }
    }
}