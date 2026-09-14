using Microsoft.AspNetCore.Mvc;
using TinyURL.Services;

namespace TinyURL.Controllers
{
    /// <summary>
    /// Handles the short-link resolution step of the workflow:
    /// GET /{code} -> look up in the in-memory dictionary -> redirect.
    /// Wired up via the "shortLink" route in Program.cs.
    /// </summary>
    public class RedirectController : Controller
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public RedirectController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpGet]
        public IActionResult Go(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) &&
                _urlShortenerService.TryGetOriginalUrl(code, out var originalUrl) &&
                !string.IsNullOrEmpty(originalUrl))
            {
                return Redirect(originalUrl);
            }

            return NotFound("Short URL not found. It may have expired or the application may have restarted.");
        }
    }
}
