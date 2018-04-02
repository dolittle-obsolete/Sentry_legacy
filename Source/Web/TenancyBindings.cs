/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Concepts;
using Dolittle.DependencyInversion;
using Read.Management;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class TenancyBindings : ICanProvideBindings
    {
        /// <inheritdoc/>
        public void Provide(IBindingProviderBuilder builder)
        {
            builder.Bind<Tenant>().To(() => {
                var pathBase = Startup.HttpContext.Request.PathBase;
                var segments = pathBase.Value.Split('/');
                var tenantId = (TenantId)Guid.Parse(segments[1]);
                var tenantConfiguration = Startup.ServiceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                var tenant = tenantConfiguration.GetFor(tenantId);
                return tenant;
            });
        }
    }
}