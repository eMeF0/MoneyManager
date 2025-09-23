using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required,MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; } = string.Empty;
    }
}
