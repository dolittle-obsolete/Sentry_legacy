
using Dolittle.Commands.Handling;
using Dolittle.Domain;
using Domain.Registration.Consents;

namespace Domain.Registration
{
    /// <summary>
    /// Represents the <see cref="Dolittle.Commands.ICommand"/> handlers for commands that has to do with user registration
    /// </summary>
    public class RegistrationCommandHandler : ICanHandleCommands
    {
        readonly IAggregateRootRepositoryFor<Registration> _aggregateRootRepository;

        /// <summary>
        /// Instantiates an instance of <see cref="RegistrationCommandHandler"/>
        /// </summary>
        /// <param name="aggregateRoot"></param>
        public RegistrationCommandHandler(IAggregateRootRepositoryFor<Registration> aggregateRoot)
        {
            _aggregateRootRepository = aggregateRoot; 
        }

        /// <summary>
        /// Handles the <see cref="GrantConsent"/> command
        /// </summary>
        /// <param name="cmd"></param>    
        public void Handle(GrantConsent cmd)
        {
            var aggregateRoot = _aggregateRootRepository.Get(cmd.Tenant.Value);
            aggregateRoot.GrantConsent(cmd.Scopes, cmd.ReturnUrl, cmd.RememberConsent);
        }
    }
}