using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTLWEB.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using BTLWEB.ViewModel;

namespace BTLWEB.Controllers
{
    //[Authorize(Roles="Admin")]
    public class BooksController : Controller
    {

        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book.ToListAsync());
        }

        // GET: Books/Details/5
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);

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



        // GET: Books/Create
        public IActionResult Create()
        {
            // Pass categories to the view
            ViewBag.Categories = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Language,ISBN,DatePublished,Price,Author,ImageUrl,CategoryIds")] Book book)
        {
            //if (ModelState.IsValid)
            //{

            //}
            _context.Add(book);
            await _context.SaveChangesAsync();

            // Associate book with categories
            if (book.CategoryIds != null)
            {
                foreach (var categoryId in book.CategoryIds)
                {
                    _context.BookCategory.Add(new BookCategory { BookId = book.Id, CategoryId = categoryId });
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
            // Repopulate ViewBag.Categories if there's a validation error
            ViewBag.Categories = new SelectList(_context.Category, "Id", "Name");
            return View(book);
        }


        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            var selectedCategory = _context.BookCategory.Where(a => a.BookId == id).Select(a => a.CategoryId).ToList();
            MultiSelectList multiCategoryList = new MultiSelectList(_context.Category.AsQueryable(), "Id", "Name", selectedCategory);
            book.MultiCategoryList = multiCategoryList;
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Language,ISBN,DatePublished,Price,Author,ImageUrl,CategoryIds")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            try
            {
                // Remove categories that are not selected anymore
                var categoriesToRemove = _context.BookCategory.Where(bc => bc.BookId == id && !book.CategoryIds.Contains(bc.CategoryId));
                _context.BookCategory.RemoveRange(categoriesToRemove);

                // Add new categories
                foreach (var categoryId in book.CategoryIds)
                {
                    if (!_context.BookCategory.Any(bc => bc.BookId == id && bc.CategoryId == categoryId))
                    {
                        _context.BookCategory.Add(new BookCategory { BookId = id, CategoryId = categoryId });
                    }
                }

                // Update book information
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            return View(book);
        }


        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book != null)
            {
                _context.Book.Remove(book);
                var bookGenres = _context.BookCategory.Where(a => a.BookId == book.Id);
                foreach (var bookGenre in bookGenres)
                {
                    _context.Remove(bookGenre);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
