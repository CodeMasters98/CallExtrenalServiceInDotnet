using CallExtrenalServiceInDotnet.Services;
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
