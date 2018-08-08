using System.Globalization;
using System.Security.Claims;
using Dolittle.Applications;
using Dolittle.DependencyInversion;
using Dolittle.Events.Storage;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.ReadModels;
using Dolittle.ReadModels.MongoDB;
using Dolittle.Runtime.Events.Processing;
using Dolittle.Runtime.Events.Publishing;
using Dolittle.Runtime.Events.Publishing.InProcess;
using Dolittle.Runtime.Events.Storage;
using Dolittle.Runtime.Execution;
using Dolittle.Security;

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
            builder.Bind<IEventStore>().To<NullEventStore>().Singleton();
            builder.Bind<IEventSourceVersions>().To<NullEventSourceVersions>().Singleton();
            builder.Bind<IEventEnvelopes>().To<EventEnvelopes>();
            builder.Bind<IEventSequenceNumbers>().To<NullEventSequenceNumbers>();
            builder.Bind<IEventProcessors>().To<EventProcessors>();
            builder.Bind<IEventProcessorLog>().To<NullEventProcessorLog>();
            builder.Bind<IEventProcessorStates>().To<NullEventProcessorStates>();
            //builder.Bind<ICanSendCommittedEventStream>().To<NullCommittedEventStreamSender>().Singleton();

            var bridge = new CommittedEventStreamBridge();
            builder.Bind<ICommittedEventStreamBridge>().To(bridge);

            builder.Bind<ICanSendCommittedEventStream>().To<CommittedEventStreamSender>().Singleton();

            var receiver = new CommittedEventStreamReceiver(bridge, new NullLogger());
            builder.Bind<ICanReceiveCommittedEventStream>().To(receiver);

            builder.Bind<ExecutionContextPopulator>().To((ExecutionContext, details)=> { });
            builder.Bind<ClaimsPrincipal>().To(()=> new ClaimsPrincipal(new ClaimsIdentity()));
            builder.Bind<CultureInfo>().To(()=> CultureInfo.InvariantCulture);
            builder.Bind<ICallContext>().To(new DefaultCallContext());
            builder.Bind<ICanResolvePrincipal>().To(new DefaultPrincipalResolver());

            builder.Bind<Dolittle.ReadModels.MongoDB.Configuration>().To(new Dolittle.ReadModels.MongoDB.Configuration
            {
                Url = "mongodb://localhost:27017",
                UseSSL = false,
                DefaultDatabase = "Demo"
            });
            builder.Bind(typeof(IReadModelRepositoryFor<>)).To(typeof(ReadModelRepositoryFor<>));
        }
    }
}