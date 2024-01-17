using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SpendWiseWebApp.Models;

public class Transaction
{
    [Key] public int Id { get; set; }

    public string? Title { get; set; }

    public float Amount { get; set; }

    public bool IsIncome { get; set; }

    public DateTime CreationTime { get; set; } = DateTime.Now;

    public Category? Category { get; set; }

    public int? CategoryId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    public int? UserId { get; set; }
}