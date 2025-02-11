using APICatalogo.Context;

namespace APICatalogo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;
    private readonly AppDbContext _context;
    
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);
    public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
    
    public Task CommitAsync()
    {
        _context.SaveChangesAsync();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}