using CallExtrenalServiceInDotnet.Models;
using Refit;

namespace CallExtrenalServiceInDotnet.Services;

public interface IProductApi
{
    [Get("/objects")]
    Task<List<Product>> GetProductsAsync();
}
