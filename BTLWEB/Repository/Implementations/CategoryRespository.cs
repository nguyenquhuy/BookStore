using BTLWEB.Models;
using BTLWEB.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BTLWEB.Repository.Implementations
{
    public class CategoryRespository : ICategoryRespository
    {
        private readonly BookContext _context;

        public CategoryRespository(BookContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categories = await _context.Category.ToListAsync();
            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Category.FindAsync(id);
        }
        public async Task Add(Category model)
        {
            await _context.Category.AddAsync(model);
            await _context.SaveChangesAsync();
        }


        public async Task Update(Category model)
        {
            var category = await _context.Category.FindAsync(model.Id);
            if(category != null) 
            {
                category.Name = model.Name;
                _context.Update(category);
                await Save();
            }            
        }

        public async Task Delete(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if(category != null )
            {
                _context.Category.Remove(category);
                await Save();
            }
        }

        private async Task Save()
        {
            await _context.SaveChangesAsync();
        }

    }
}
