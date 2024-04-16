using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTLWEB.Models
{
    public class BookContext : IdentityDbContext<DefaultUser>
    {
        public BookContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Book { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Category { get; set; }

        public DbSet<BookCategory> BookCategory { get; set; }
        
        
    }
}
