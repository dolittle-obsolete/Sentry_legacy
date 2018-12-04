using System.Security.Claims;
using Dolittle.DependencyInversion;
using Read;

namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Bindings : ICanProvideBindings
    {
        /// <inheritdoc/>
        public void Provide(IBindingProviderBuilder builder)
        {
            builder.Bind<ClaimsPrincipal>().To(()=> new ClaimsPrincipal(new ClaimsIdentity()));
            builder.Bind<ITenantConfiguration>().To<TenantTestConfiguration>();
        }
    }
}