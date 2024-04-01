using Microsoft.EntityFrameworkCore;

namespace BTLWEB.Models
{
    public class Cart
    {
        public readonly BookContext _context;

        public Cart(BookContext context)
        {
            _context = context;
        }

        public string? Id { get; set; }
        public List<CartItems> CartItems { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<BookContext>();
            string cardId = session.GetString("Id") ?? Guid.NewGuid().ToString();

            session.SetString("Id", cardId);

            return new Cart(context) { Id = cardId };
        }
        public CartItems GetCartItems(Book book) {
            return _context.CartItems.SingleOrDefault(
                ci => ci.Book.Id == book.Id && ci.CartId == Id);
        }
        public void AddToCart(Book book, int quantity) {
            var cartItem = GetCartItems(book);
            if(cartItem == null)
            {
                cartItem = new CartItems
                {
                    Book = book,
                    CartId = Id,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            _context.SaveChanges();
        }

        public int ReduceQuantity(Book book)
        {
            var cartItem = GetCartItems(book);
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

        public int IncreaseQuantity(Book book)
        {
            var cartItem = GetCartItems(book);
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

        public void RemoveCart(Book book)
        {
            var cartItem = GetCartItems(book);
            if(cartItem != null)
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
