using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyManager.Models;

namespace MoneyManager.ViewModels;

public class TransactionFormViewModel
{
    public int Id { get; set; }

    [Required]
    [Range(0.01, 999999999)]
    public decimal Amount { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required]
    public TransactionType Type { get; set; } = TransactionType.Expense;

    [StringLength(150)]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = [];
}
