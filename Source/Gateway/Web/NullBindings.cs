using System.Globalization;
using System.Security.Claims;
using Dolittle.Applications;
using Dolittle.DependencyInversion;
using Dolittle.Events.Coordination;
using Dolittle.Execution;
using Dolittle.ReadModels;
using Dolittle.Runtime.Events.Coordination;
using Dolittle.Runtime.Events.Store;
using Dolittle.Runtime.Execution;
using Dolittle.Security;
using Infrastructure;
using MongoDB.Driver;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class NullBindings : ICanProvideBindings
    {
        /// <inheritdoc/>
        public void Provide(IBindingProviderBuilder builder)
        {
            builder.Bind<ICallContext>().To<DefaultCallContext>();
            builder.Bind<ExecutionContextPopulator>().To((ExecutionContext, details)=> { });

            builder.Bind<ClaimsPrincipal>().To(()=> new ClaimsPrincipal(new ClaimsIdentity()));
            builder.Bind<CultureInfo>().To(()=> CultureInfo.InvariantCulture);

            builder.Bind<ICanResolvePrincipal>().To(new DefaultPrincipalResolver());
            
            var client = new MongoClient("mongodb://localhost:27017"); 
            var database = client.GetDatabase("Shop_EventStore");
            builder.Bind<IMongoDatabase>().To(database);

            builder.Bind<IEventStore>().To<Dolittle.Runtime.Events.Store.MongoDB.EventStore>();
            builder.Bind<IUncommittedEventStreamCoordinator>().To<UncommittedEventStreamCoordinator>();

            builder.Bind<Dolittle.ReadModels.MongoDB.Configuration>().To(new Dolittle.ReadModels.MongoDB.Configuration
            {
                Url = "mongodb://localhost:27017",
                UseSSL = false,
                DefaultDatabase = "Gateway"
            });

            builder.Bind(typeof(IReadModelRepositoryFor<>)).To(typeof(Dolittle.ReadModels.MongoDB.ReadModelRepositoryFor<>));

            builder.Bind<ITenantConfiguration>().To<TenantConfiguration>();
        }
    }
}