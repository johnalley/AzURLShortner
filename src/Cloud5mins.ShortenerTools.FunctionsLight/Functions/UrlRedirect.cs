using Cloud5mins.ShortenerTools.Core.Services;
using Cloud5mins.ShortenerTools.Core.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud5mins.ShortenerTools.Functions
{
    public class UrlRedirect
    {
        private readonly ILogger _logger;
        private readonly ShortenerSettings _settings;

        public UrlRedirect(ILoggerFactory loggerFactory, ShortenerSettings settings)
        {
            console.log("UrlRedirect constructor");
            _logger = loggerFactory.CreateLogger<UrlRedirect>();
            _logger.LogInformation("UrlRedirect in constructor");
            _logger.LogInformation("DataStorage: " + settings.DataStorage.ToString());
            _settings = settings;
        }

        [Function("UrlRedirect")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{shortUrl}")]
            HttpRequestData req,
            string shortUrl,
            ExecutionContext context)
        {
            console.log("UrlRedirect starting");
            UrlServices UrlServices = new UrlServices(_settings, _logger);
            console.log("Services created");
            _logger.LogInformation($"Redirecting {shortUrl}");
            string redirectUrl = await UrlServices.Redirect(shortUrl);
            console.log("got the redirect url");
            var res = req.CreateResponse(HttpStatusCode.Redirect);
            res.Headers.Add("Location", redirectUrl);
            console.log("response created");
            return res;

        }
    }
}
