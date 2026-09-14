using System.Collections.Concurrent;

namespace TinyURL.Services
{
    /// <summary>
    /// Stores short-code -> long-URL mappings in memory only.
    /// Registered as a Singleton so the same dictionary instance lives for
    /// the lifetime of the application (data is lost on restart by design —
    /// see "Limitations" in the project write-up).
    /// </summary>
    public class UrlShortenerService : IUrlShortenerService
    {
        // ConcurrentDictionary so multiple simultaneous requests can safely
        // read/write without an explicit lock.
        private readonly ConcurrentDictionary<string, string> _urlMap = new();

        private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int CodeLength = 6;

        public string ShortenUrl(string originalUrl)
        {
            // If this exact URL was already shortened, return the existing
            // code instead of creating a duplicate mapping.
            foreach (var pair in _urlMap)
            {
                if (string.Equals(pair.Value, originalUrl, StringComparison.OrdinalIgnoreCase))
                {
                    return pair.Key;
                }
            }

            string code;
            do
            {
                code = GenerateRandomCode();
            }
            while (!_urlMap.TryAdd(code, originalUrl));

            return code;
        }

        public bool TryGetOriginalUrl(string code, out string? originalUrl)
        {
            return _urlMap.TryGetValue(code, out originalUrl);
        }

        private static string GenerateRandomCode()
        {
            Span<char> buffer = stackalloc char[CodeLength];
            for (int i = 0; i < CodeLength; i++)
            {
                // Random.Shared is thread-safe (available since .NET 6).
                buffer[i] = AllowedChars[Random.Shared.Next(AllowedChars.Length)];
            }

            return new string(buffer);
        }
    }
}
