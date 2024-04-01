namespace BTLWEB.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        public Book? Book { get; set; }
        public int Quantity { get; set; }
        public string? CartId { get; set; }
    }
}
