using BTLWEB.Models;

namespace BTLWEB.Repository.Interfaces
{
    public interface ICategoryRespository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task Add(Category model);
        Task Update(Category model);
        Task Delete(int id);
    }
}
