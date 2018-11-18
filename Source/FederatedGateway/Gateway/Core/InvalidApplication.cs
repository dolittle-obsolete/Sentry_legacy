/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Runtime.Serialization;
using Read.Infrastructure.Gateway.Tenants;

namespace Core
{
    internal class InvalidApplication : Exception
    {
        public InvalidApplication(Tenant tenant, string applicationName)
            : base($"Application '{applicationName}' does not exist under tenant '{tenant.TenantId.Value.ToString()}'")
        {
        }
    }
}