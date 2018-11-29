using System.Collections.Generic;
using Concepts;
using Dolittle.Lifecycle;
using Read;
using Read.Tenants;

namespace Core
{
    /// <inheritdoc/>
    [Singleton]
    public class TenantTestConfiguration : ITenantConfiguration
    {
        readonly IDictionary<TenantId, Tenant> _tenants = new Dictionary<TenantId, Tenant>();

        readonly ITenantConfigurationManager _tenantConfigurationManager;
        /// <inheritdoc/>
        public TenantTestConfiguration(ITenantConfigurationManager tenantConfigurationManager)
        {
            _tenantConfigurationManager = tenantConfigurationManager;
            _tenantConfigurationManager.Path = "508c1745-5f2a-4b4c-b7a5-2fbb1484346d.json";

            var testTenant = _tenantConfigurationManager.Load();
            _tenants.Add(testTenant.TenantId, testTenant);

        }
        /// <inheritdoc/>
        public Tenant GetFor(TenantId tenantId)
        {
            return _tenants[tenantId];
        }
        /// <inheritdoc/>
        public bool HasTenant(TenantId tenantId)
        {
            return _tenants.ContainsKey(tenantId);
        }
        /// <inheritdoc/>
        public void Save(Tenant tenant)
        {
            _tenantConfigurationManager.Save(tenant);
        }
    }
}