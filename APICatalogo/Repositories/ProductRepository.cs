using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<Product> GetProducts()
    {
        return _context.Products;
    }

    public Product GetProduct(int id)
    {
        var product = _context.Products.Find(id);

        if (product is null)
            throw new InvalidOperationException($"Não foi encontrado um produto com o ID -{id}- informado.");
        
        return product;
    }

    public Product Create(Product product)
    {
        if (product is null)
            throw new InvalidOperationException($"Produto é nulo");
        
        _context.Products.Add(product);
        _context.SaveChanges();
        return product;
    }

    public bool Update(Product product)
    {
        if (product is null)
            throw new InvalidOperationException($"Produto é nulo");

        if (!_context.Products.Any(p => p.Id == product.Id)) return false;
        
        _context.Products.Update(product);
        _context.SaveChanges();
        return true;

    }

    public bool Delete(int id)
    {
        var product = _context.Products.Find(id);

        if (product is null) return false;
        
        _context.Products.Remove(product);
        _context.SaveChanges();
        return true;

    }
}
