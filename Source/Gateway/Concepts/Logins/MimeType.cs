using Dolittle.Concepts;

namespace Concepts.Logins
{
    public class MimeType : ConceptAs<string>
    {
        public static readonly string NotSet = string.Empty;

        public static implicit operator MimeType(string mimeType)
        {
            return new MimeType { Value = mimeType };
        }
    }
}