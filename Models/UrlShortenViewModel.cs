using System.ComponentModel.DataAnnotations;

namespace TinyURL.Models
{
    /// <summary>
    /// View model bound to the home page form. Carries the long URL the
    /// user submits and, once processed, the resulting short URL.
    /// </summary>
    public class UrlShortenViewModel
    {
        [Required(ErrorMessage = "Please enter a URL to shorten.")]
        [Url(ErrorMessage = "Please enter a valid URL, e.g. https://example.com/page.")]
        [Display(Name = "Long URL")]
        public string OriginalUrl { get; set; } = string.Empty;

        /// <summary>
        /// Populated after a successful POST. Null/empty on the initial GET.
        /// </summary>
        public string? ShortUrl { get; set; }
    }
}
