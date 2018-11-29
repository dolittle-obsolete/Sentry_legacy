using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Core
{
    public static class HttpRequestExtensions
    {
        public static StringValues GetEtag(this HttpRequest request)
        {
            StringValues values = "";
            request.Headers.TryGetValue("If-None-Match", out values);

            return values;
        }
    }
}