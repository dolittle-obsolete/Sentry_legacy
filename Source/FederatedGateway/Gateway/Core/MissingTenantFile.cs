using System;
using System.Runtime.Serialization;

namespace Core
{
    /// <summary>
    /// The Exception that gets thrown when the <see cref="TenantConfigurationManager"/> attempts to read or write a tenant from a file that does not exist
    /// </summary>
    public class MissingTenantFile : Exception
    {
        /// <summary>
        /// Instantiates an instance of <see cref="MissingTenantFile"/>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public MissingTenantFile(string path) : base($"Tried to load Tenant from path = {path}, but that path doesn't exist")
        {
        }
    }
}