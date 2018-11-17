using System.Globalization;
using System.Security.Claims;
using Dolittle.Applications;
using Dolittle.DependencyInversion;
using Dolittle.Events.Coordination;
using Dolittle.Execution;
using Dolittle.ReadModels;
using Dolittle.Runtime.Events.Coordination;
using Dolittle.Runtime.Events.Store;
using Dolittle.Security;
using MongoDB.Driver;
using Read.Infrastructure.Gateway;

namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    public class NullBindings : ICanProvideBindings
    {
        /// <inheritdoc/>
        public void Provide(IBindingProviderBuilder builder)
        {
            builder.Bind<ClaimsPrincipal>().To(()=> new ClaimsPrincipal(new ClaimsIdentity()));
            builder.Bind<ITenantConfiguration>().To<TenantTestConfiguration>();
        }
    }
}