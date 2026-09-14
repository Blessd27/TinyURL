namespace TinyURL.Services
{
    /// <summary>
    /// Contract for generating and resolving short URL codes.
    /// </summary>
    public interface IUrlShortenerService
    {
        /// <summary>
        /// Generates (or reuses) a short code for the given long URL and
        /// stores the mapping in memory.
        /// </summary>
        string ShortenUrl(string originalUrl);

        /// <summary>
        /// Attempts to resolve a short code back to its original long URL.
        /// </summary>
        bool TryGetOriginalUrl(string code, out string? originalUrl);
    }
}
