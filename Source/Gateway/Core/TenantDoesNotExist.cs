/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Tenancy;

namespace Core
{
    public class TenantDoesNotExist : Exception
    {
        public TenantDoesNotExist(TenantId tenantId) : base($"Tenant with id '{tenantId.Value.ToString()}' does not exist")
        { 
        }
    }
}