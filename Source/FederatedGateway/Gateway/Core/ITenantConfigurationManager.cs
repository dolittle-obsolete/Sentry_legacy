using System.Collections.Generic;
using Read.Infrastructure.Gateway.Tenants;

namespace Core
{
    /// <summary>
    /// Represents a class that manages tenant file
    /// </summary>
    public interface ITenantConfigurationManager
    {
        /// <summary>
        /// The file path of the tenant.json file
        /// </summary>
        /// <value></value>
        string Path {get; set;}

        /// <summary>
        /// Reads the <see cref="Tenant"/> from the tenant file
        /// </summary>
        /// <returns></returns>
        Tenant Load();
        
        /// <summary>
        /// Writes the <see cref="Tenant"/> to file
        /// </summary>
        /// <param name="tenant">The <see cref="Tenant"/>to write to file</param>
        void Save(Tenant tenant);
    }
}