using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BTLWEB.Models
{
    public class Cart
    {
        private readonly BookContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<DefaultUser> _userManager;

        public Cart(BookContext context, IHttpContextAccessor httpContextAccessor, UserManager<DefaultUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string Id { get; set; }
        public List<CartItems> CartItems { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<BookContext>();
            string cartId = session.GetString("Id") ?? Guid.NewGuid().ToString();

            session.SetString("Id", cartId);

            return new Cart(context, services.GetRequiredService<IHttpContextAccessor>(), services.GetRequiredService<UserManager<DefaultUser>>()) { Id = cartId };
        }

        public CartItems GetCartItems(Book book, string userId)
        {
            return _context.CartItems.SingleOrDefault(ci => ci.Book.Id == book.Id && ci.CartId == Id && ci.UserId == userId);
        }
        public void AddToCart(Book book, int quantity, string userId)
        {
            var cartItem = GetCartItems(book, userId);
            if (cartItem == null)
            {
                cartItem = new CartItems
                {
                    Book = book,
                    CartId = Id,
                    Quantity = quantity,
                    UserId = userId
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            _context.SaveChanges();
        }

        public int ReduceQuantity(Book book, string userId)
        {
            var cartItem = GetCartItems(book, userId);
            var remainingQuantity = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    remainingQuantity = --cartItem.Quantity;
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }
            }
            _context.SaveChanges();

            return remainingQuantity;
        }

        public int IncreaseQuantity(Book book, string userId)
        {
            var cartItem = GetCartItems(book, userId);
            var remainingQuantity = 0;
            if (cartItem != null)
            {
                if (cartItem.Quantity > 0)
                {
                    remainingQuantity = ++cartItem.Quantity;
                }
            }
            _context.SaveChanges();

            return remainingQuantity;
        }

        public void RemoveCart(Book book, string userId)
        {
            var cartItem = GetCartItems(book, userId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
            }
            _context.SaveChanges();
        }

        public void ClearCart()
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartId == Id);

            _context.CartItems.RemoveRange(cartItems);

            _context.SaveChanges();
        }

        public List<CartItems> GetAllCartItems()
        {
            return CartItems ??
                (CartItems = _context.CartItems.Where(ci => ci.CartId == Id)
                .Include(ci => ci.Book)
                .ToList());
        }

        public int GetCartTotal()
        {
            return _context.CartItems.Where(ci => ci.CartId == Id)
                .Select(ci => ci.Book.Price * ci.Quantity).Sum();
        }
    }
}
