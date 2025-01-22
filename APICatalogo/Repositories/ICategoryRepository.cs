using System.Collections;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> GetCategries();
    Category GetCategory(int id);
    Category Create(Category category);
    Category Update(Category category);
    Category Delete(int id);
}