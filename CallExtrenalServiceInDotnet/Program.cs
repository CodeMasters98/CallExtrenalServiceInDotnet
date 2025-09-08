using CallExtrenalServiceInDotnet.Services;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace CallExtrenalServiceInDotnet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //retry policy (exponential backoff)
            static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        3,                  // retry count
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // 2s, 4s, 8s
                    );
            }

            //circuit breaker policy
            static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                        2,                  // break after 2 consecutive failures
                        TimeSpan.FromSeconds(30) // stay open for 30s
                    );
            }

            builder.Services.AddHttpClient("restful", (serviceProvider, client) =>
            {
                //var settings = serviceProvider
                //    .GetRequiredService<IOptions<SampleSetting>>().Value;

                //client.DefaultRequestHeaders.Add("Authorization", settings.GitHubToken);
                //client.DefaultRequestHeaders.Add("User-Agent", settings.UserAgent);

                client.BaseAddress = new Uri("https://api.restful-api.dev");
            });

            builder.Services.AddHttpClient<ProductService>((serviceProvider, client) =>
            {
                //var settings = serviceProvider
                //    .GetRequiredService<IOptions<SampleSetting>>().Value;

                //client.DefaultRequestHeaders.Add("Authorization", settings.GitHubToken);
                //client.DefaultRequestHeaders.Add("User-Agent", settings.UserAgent);

                client.BaseAddress = new Uri("https://api.restful-api.dev");
            });

            builder.Services
                .AddRefitClient<IProductApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.restful-api.dev"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
