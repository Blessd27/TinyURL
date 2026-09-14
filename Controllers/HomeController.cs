using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TinyURL.Models;
using TinyURL.Services;

namespace TinyURL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public HomeController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        // GET: /
        [HttpGet]
        public IActionResult Index()
        {
            return View(new UrlShortenViewModel());
        }

        // POST: /
        // Validates the submitted URL, generates a short code, stores the
        // mapping in the in-memory dictionary, and renders the result.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UrlShortenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!Uri.TryCreate(model.OriginalUrl, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                ModelState.AddModelError(
                    nameof(model.OriginalUrl),
                    "Please enter a valid URL starting with http:// or https://");
                return View(model);
            }

            var code = _urlShortenerService.ShortenUrl(model.OriginalUrl);

            var request = HttpContext.Request;
            model.ShortUrl = $"{request.Scheme}://{request.Host}/{code}";

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
