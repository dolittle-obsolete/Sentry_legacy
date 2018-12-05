using Dolittle.Concepts;

namespace Concepts.Logins
{
    public class Logo : ConceptAs<byte[]>
    {
        public static implicit operator Logo(byte[] byteArray)
        {
            return new Logo { Value = byteArray };
        }
    }
}