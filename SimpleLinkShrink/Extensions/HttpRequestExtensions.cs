﻿namespace SimpleLinkShrink.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri GetBaseUrl(this HttpRequest request)
        {
            return new Uri($"{request.Scheme}{Uri.SchemeDelimiter}{request.Host}{request.PathBase}");
        }
    }
}
