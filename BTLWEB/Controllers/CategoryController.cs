using BTLWEB.Models;
using BTLWEB.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTLWEB.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRespository _categoryRepo;
        private readonly BookContext _context;

        public CategoryController(ICategoryRespository categoryRepo, BookContext context)
        {
            _categoryRepo = categoryRepo;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetAll();
            return View(categories);
        }

        public async Task<IActionResult> Search(string category)
        {
            var booksInCategory = from book in _context.Book
                                  join bookCategory in _context.BookCategory on book.Id equals bookCategory.BookId
                                  join cat in _context.Category on bookCategory.CategoryId equals cat.Id
                                  where cat.Name == category
                                  select book;

            var books = await booksInCategory.ToListAsync();
            ViewData["Search"] = category; // Set the category name in ViewData

            return View(books); // Pass the list of books to the view
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category model) 
        {
            try
            {
                await _categoryRepo.Add(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepo.GetById(id);
            return View(category);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            try
            {
                await _categoryRepo.Update(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _categoryRepo.GetById(id);
            if(category != null)
            {
                return View(category);
            }
            TempData["errorMessage"] = $"Category details not found with Id: {id}";
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
