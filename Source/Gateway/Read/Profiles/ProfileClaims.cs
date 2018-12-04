/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Linq;
using Concepts;
using Dolittle.Queries;
using Read;

namespace Read.Profiles
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfileClaims : IQueryFor<ProfileClaim>
    {
        readonly ITenantConfiguration _tenantConfiguration;

        /// <summary>
        /// Initializes a new instance of <see cref="ProfileClaims"/>
        /// </summary>
        /// <param name="tenantConfiguration"></param>
        public ProfileClaims(ITenantConfiguration tenantConfiguration)
        {
            _tenantConfiguration = tenantConfiguration;
        }

        /// <summary>
        /// 
        /// </summary>
        public TenantId Tenant {  get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<ProfileClaim> Query
        {
            get
            {
                var tenant = _tenantConfiguration.GetFor(Tenant);
                return tenant.Applications[Application].ProfileClaims.AsQueryable();
            }
        }
    }
}