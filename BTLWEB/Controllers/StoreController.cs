using BTLWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTLWEB.Controllers
{
    [AllowAnonymous]
    public class StoreController : Controller
    {
        public readonly BookContext _context;

        public StoreController(BookContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            int pageSize = 4; // Set the page size to 4 items per page

            var books = _context.Book.Select(b => b);

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            var totalCount = await books.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            books = books.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(await books.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            var categories = await (from category in _context.Category
                                    join bc in _context.BookCategory
                                    on category.Id equals bc.CategoryId
                                    where bc.BookId == id
                                    select category.Name
                                    ).ToListAsync();

            ViewData["Categories"] = categories;

            return View(book);
        }
    }
}
