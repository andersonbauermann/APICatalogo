using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetByCategory(int id);
}