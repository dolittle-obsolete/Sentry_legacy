/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Runtime.Serialization;

namespace Core
{
    public class InvalidTenantId : Exception
    {
        public InvalidTenantId() : base("Invalid Tenant Id")
        {
        }

        public InvalidTenantId(string message) : base(message)
        {
        }
    }
}