using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Range(0.01, 999999999)]
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public TransactionType Type { get; set; }

        [MaxLength(150)]
        public string? Description { get; set; } = string.Empty;

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        // Foreign key to Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
