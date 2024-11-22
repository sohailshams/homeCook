using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface ICategoriesReposity
    {
        Task<List<Category>> GetCategoriesAsync();
    }
}