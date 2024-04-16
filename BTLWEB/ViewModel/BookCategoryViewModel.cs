using BTLWEB.Models;

namespace BTLWEB.ViewModel
{
    public class BookCategoryViewModel
    {
        public IQueryable<Book> BookList { get; set; }
    }
}
