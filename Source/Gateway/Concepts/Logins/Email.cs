using Dolittle.Concepts;

namespace Concepts.Logins
{
    public class Email : ConceptAs<string>
    {
        public static readonly Email NotSet = string.Empty;

        public static implicit operator Email(string email)
        {
            return new Email { Value = email.ToLower().Trim() };
        }
    }
}