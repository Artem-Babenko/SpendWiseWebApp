using System.ComponentModel.DataAnnotations;

namespace SpendWiseWebApp.Models;

public class Category
{
    [Key] public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsIncome { get; set; }

    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
}
