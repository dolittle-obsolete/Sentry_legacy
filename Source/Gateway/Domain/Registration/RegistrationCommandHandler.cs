using Dolittle.Commands.Handling;
using Dolittle.Domain;
using Domain.Registration.Consents;

namespace Domain.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class RegistrationCommandHandler : ICanHandleCommands
    {
        readonly IAggregateRootRepositoryFor<Registration> _aggregateRoot;

        public RegistrationCommandHandler(IAggregateRootRepositoryFor<Registration> aggregateRoot)
        {
            _aggregateRoot = aggregateRoot; 
        }

        public void Handle(GrantConsent cmd)
        {
            var aggregate = _aggregateRoot.Get(cmd.Tenant.Value);
            aggregate.GrantConsent(cmd.Application, cmd.ClientName, cmd.IdentityScopesConsentedTo, cmd.ResourceScopesConsentedTo);
        }
    }
}