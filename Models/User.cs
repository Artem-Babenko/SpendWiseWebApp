using System.ComponentModel.DataAnnotations;

namespace SpendWiseWebApp.Models;

public class User
{
    [Key] public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
}
