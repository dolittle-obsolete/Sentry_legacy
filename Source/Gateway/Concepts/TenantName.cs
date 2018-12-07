﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Concepts
{
    public class TenantName : ConceptAs<string>
    {
        public static readonly TenantName NotSet = string.Empty;

        public static implicit operator TenantName(string tenantName)
        {
            return new TenantName { Value = tenantName };
        }
    }
}
