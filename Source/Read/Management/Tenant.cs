/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Concepts;

namespace Read.Management
{
    /// <summary>
    /// Represents the configuration for a tenant
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Tenant"/>
        /// </summary>
        public Tenant() 
        {
            Applications = new Dictionary<string, Application>();
        }

        /// <summary>
        /// Gets or sets the unique identifier of the tenant
        /// </summary>
        public TenantId TenantId { get; set; }

        /// <summary>
        /// Gets or sets name of the tenant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the configuration of <see cref="Application">applications</see> 
        /// </summary>
        public Dictionary<string, Application> Applications { get; set; }
    }
}