/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Entrypoint
{
    /// <summary>
    /// Represents a <see cref="OpenIdConnectExternalAuthority"/> for Azure Active Directory
    /// </summary>
    public class AzureActiveDirectoryExternalAuthority : OpenIdConnectExternalAuthority
    {
        /// <summary>
        /// Gets or sets the unique identifier that identifies the tenant that owns the Azure Active Directory
        /// </summary>
        public Guid TenantId { get; set; }
    }
}