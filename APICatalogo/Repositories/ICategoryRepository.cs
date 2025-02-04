using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    public PagedList<Category> GetCategeories(PaginationParameters paginationParameters);
}