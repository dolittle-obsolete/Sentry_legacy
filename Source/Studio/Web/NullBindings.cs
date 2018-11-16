using System.Globalization;
using System.Security.Claims;
using Dolittle.Applications;
using Dolittle.DependencyInversion;
using Dolittle.Events.Coordination;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.ReadModels;
using Dolittle.ReadModels.MongoDB;
using Dolittle.Runtime.Events.Coordination;
using Dolittle.Runtime.Events.Store;
using Dolittle.Security;
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
            builder.Bind<ClaimsPrincipal>().To(()=> new ClaimsPrincipal(new ClaimsIdentity()));
            builder.Bind<CultureInfo>().To(()=> CultureInfo.InvariantCulture);

            builder.Bind<ICanResolvePrincipal>().To(new DefaultPrincipalResolver());
            
            var client = new MongoClient("mongodb://localhost:27017"); 
            var database = client.GetDatabase("Studio_EventStore");
            builder.Bind<IMongoDatabase>().To(database);

            builder.Bind<IEventStore>().To<Dolittle.Runtime.Events.Store.MongoDB.EventStore>();
            builder.Bind<IUncommittedEventStreamCoordinator>().To<UncommittedEventStreamCoordinator>();

            builder.Bind<Dolittle.ReadModels.MongoDB.Configuration>().To(new Dolittle.ReadModels.MongoDB.Configuration
            {
                Url = "mongodb://localhost:27017",
                UseSSL = false,
                DefaultDatabase = "Studio"
            });

            builder.Bind(typeof(IReadModelRepositoryFor<>)).To(typeof(Dolittle.ReadModels.MongoDB.ReadModelRepositoryFor<>));
        }
    }
}