namespace MoneyManager.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        // Foreign key to Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }


    }
}
