using APICatalogo.Context;

namespace APICatalogo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;
    public AppDbContext _context;
    
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);
    public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
    
    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}