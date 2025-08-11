using CallExtrenalServiceInDotnet.Models;
using CallExtrenalServiceInDotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace CallExtrenalServiceInDotnet.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SampleApiCallController : ControllerBase
    {
        private readonly ILogger<SampleApiCallController> _logger;
        private readonly IHttpClientFactory _factory;
        private readonly ProductService _productService;
        private readonly IProductApi _productApi;
        public SampleApiCallController(
            ILogger<SampleApiCallController> logger,
            IHttpClientFactory factory,
            ProductService productService,
            IProductApi productApi)
        {
            _logger = logger;
            _factory = factory;
            _productService = productService;
            _productApi = productApi;
        }

        [HttpGet]
        public async Task<IActionResult> DirectHttpClient()
        {
            using var client = new HttpClient();

            var products = await client.GetFromJsonAsync<List<Product>>($"objects");

            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> HttpClientByFactoryWithNamedClient()
        {
            using var client = _factory.CreateClient("restful");

            var products = await client.GetFromJsonAsync<List<Product>>($"objects");

            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> HttpClientByFactoryWithTypeClient()
        {
            var products = await _productService.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> DirectHttpClientWithCorrelation()
        {
            using var client = new HttpClient();

            //client.DefaultRequestHeaders.Add("Authorization", _settings.GitHubToken);
            client.BaseAddress = new Uri("https://api.restful-api.dev");

            // Get correlation ID from incoming HTTP context headers
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            // If missing, generate a new one
            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            // Add correlation ID header to outgoing HTTP request
            client.DefaultRequestHeaders.Add("X-Correlation-ID", correlationId);

            var products = await client.GetFromJsonAsync<List<Product>>($"objects");

            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> ByRefit()
        {
            var products = await _productApi.GetProductsAsync();
            return Ok(products);
        }
    }
}
