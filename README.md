# TinyURL — URL Shortener Web Application

A lightweight URL shortener built with **ASP.NET Core MVC**, matching the accompanying
presentation slide-by-slide.

## Tech stack (Slide 3)

| Layer     | Technology |
|-----------|------------|
| Backend   | ASP.NET Core MVC, C# |
| Frontend  | HTML5, CSS3, Bootstrap 5, JavaScript |
| Storage   | In-memory `Dictionary<string, string>` (via `ConcurrentDictionary`) — **no database** |

## Project layout

```
TinyURL/
├── Controllers/
│   ├── HomeController.cs      # GET/POST "/" — the shorten form
│   └── RedirectController.cs  # GET "/{code}" — resolves & redirects
├── Models/
│   ├── UrlShortenViewModel.cs # form input + result URL, with validation
│   └── ErrorViewModel.cs
├── Services/
│   ├── IUrlShortenerService.cs
│   └── UrlShortenerService.cs # in-memory dictionary, random code generator
├── Views/
│   ├── Home/Index.cshtml      # main page: form, result box, "how it works"
│   ├── Home/Privacy.cshtml
│   └── Shared/_Layout.cshtml  # Bootstrap 5 layout/navbar
├── wwwroot/
│   ├── css/site.css           # theme matching the deck (navy + blue)
│   └── js/site.js             # copy-to-clipboard button behavior
├── Program.cs                 # app startup, DI, routing
└── TinyURL.csproj
```

## How it implements the workflow (Slide 4)

1. **Enter Long URL** — user types a URL into the form on the home page.
2. **Validate URL** — server-side validation via `[Url]` data annotation plus an
   explicit `Uri.TryCreate` check that the scheme is `http`/`https`.
3. **Generate Random Short Code** — `UrlShortenerService` generates a random
   6-character alphanumeric code, retrying on collision.
4. **Store Mapping in Memory** — the code/URL pair is stored in a
   `ConcurrentDictionary<string, string>` registered as a **singleton** service.
5. **Generate Short URL / Copy Button** — the result box shows the short URL with a
   one-click **Copy** button (`wwwroot/js/site.js`, using the Clipboard API).
6. **Paste in Browser → Redirect to Original Site** — visiting `/{code}` hits
   `RedirectController.Go`, which looks up the code and issues an HTTP redirect
   to the original URL (404 if the code isn't found, e.g. after a restart).

## Features (Slide 5)

- Generate Short URL
- URL Validation
- Random Short Code Generator
- Copy Short URL Button
- URL Redirection
- Responsive UI (Bootstrap 5 grid + components)

## Limitations & Future Scope (Slide 7)

By design (to keep the project focused on core MVC concepts), this app has no
database, so data is lost on restart, and there's no analytics or authentication.
Natural next steps: SQL Server persistence, custom short URLs, QR code generation,
a click-analytics dashboard, user authentication, and cloud deployment.

## Running the project

Requires the **.NET 8 SDK**.

```bash
cd TinyURL
dotnet restore
dotnet run
```

Then open the URL shown in the console (e.g. `https://localhost:7285`).

> **Note:** This project was generated in a sandboxed environment without the
> .NET SDK available, so it has not been compiled/run here. The code was written
> and reviewed carefully for correctness, but please run `dotnet build` locally
> as a first step and let me know if you hit any issue — happy to fix it.
