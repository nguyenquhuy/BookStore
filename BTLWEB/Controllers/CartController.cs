using BTLWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BTLWEB.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly BookContext _context;
        private readonly Cart _cart;
        private readonly UserManager<DefaultUser> _userManager;

        public CartController(BookContext context, Cart cart, UserManager<DefaultUser> userManager)
        {
            _context = context;
            _cart = cart;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var items = _cart.GetAllCartItems();
            _cart.CartItems = items;
            return View(_cart);
        }

        public IActionResult AddToCart(int id)
        {
            var selectedBook = GetBookById(id);
            if (selectedBook != null)
            {
                var userId = _userManager.GetUserId(User);
                _cart.AddToCart(selectedBook, 1, userId);
            }
            return RedirectToAction("Index", "Store");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var selectedBook = GetBookById(id);
            if (selectedBook != null)
            {
                var userId = _userManager.GetUserId(User);
                _cart.RemoveCart(selectedBook, userId);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ReduceQuantity(int id)
        {
            var selectedBook = GetBookById(id);
            if (selectedBook != null)
            {
                var userId = _userManager.GetUserId(User);
                _cart.ReduceQuantity(selectedBook, userId);
            }
            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(int id)
        {
            var selectedBook = GetBookById(id);
            if (selectedBook != null)
            {
                var userId = _userManager.GetUserId(User);
                _cart.IncreaseQuantity(selectedBook, userId);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            var userId = _userManager.GetUserId(User);
            _cart.ClearCart();
            return RedirectToAction("Index");
        }

        public Book GetBookById(int id)
        {
            return _context.Book.FirstOrDefault(b => b.Id == id);
        }
    }

}
