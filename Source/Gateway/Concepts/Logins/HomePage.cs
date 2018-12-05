using Dolittle.Concepts;

namespace Concepts.Logins
{
    public class HomePage : ConceptAs<string>
    {
        public static readonly HomePage NotSet = string.Empty;

        public static implicit operator HomePage(string homepage)
        {
            return new HomePage { Value = homepage.ToLower().Trim() };
        }
    }
}
