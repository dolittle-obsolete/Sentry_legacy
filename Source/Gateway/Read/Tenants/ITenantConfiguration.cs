/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Concepts;
using Read.Tenants;

namespace Read
{

    /// <summary>
    /// Defines a system for working with <see cref="Tenant"/> configuration
    /// </summary>
    public interface ITenantConfiguration
    {
        /// <summary>
        /// Check if there is a tenant by a specific identifier or not
        /// </summary>
        /// <param name="tenantId"><see cref="TenantId"/> representing the Tenant</param>
        /// <returns>True if it exists, false if not</returns>
        bool HasTenant(TenantId tenantId);

        /// <summary>
        /// Gets a <see cref="Tenant"/> configuration for a specific <see cref="TenantId">tenant</see>
        /// </summary>
        /// <param name="tenant"><see cref="TenantId">Tenant</see> to get for</param>
        /// <returns><see cref="Tenant"/> configuration</returns>
        Tenant GetFor(TenantId tenant);

        /// <summary>
        /// Saves a <see cref="Tenant"/> configuration
        /// </summary>
        /// <param name="tenant"><see cref="Tenant"/> to save</param>
        void Save(Tenant tenant);
    }
}