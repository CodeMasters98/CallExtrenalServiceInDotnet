using CallExtrenalServiceInDotnet.Models;

namespace CallExtrenalServiceInDotnet.Services;

public class ProductService
{
    private readonly HttpClient _client;

    public ProductService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Product>?> GetProductsAsync()
    {
        List<Product>? user = await _client.GetFromJsonAsync<List<Product>>($"objects");

        return user;
    }
}
