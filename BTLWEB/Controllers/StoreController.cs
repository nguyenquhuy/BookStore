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

        public async Task<IActionResult> Index(string searchString, string minPrice, string maxPrice)
        {
            var books = _context.Book.Select(b => b);

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

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

            return View(book);
        }
    }
}
