using Microsoft.EntityFrameworkCore;
namespace SpendWiseWebApp.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Transaction> Transactions { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Establishing a connection between one User and many Transactions.
        modelBuilder.Entity<Transaction>()
            .HasOne(transaction => transaction.User)
            .WithMany(user => user.Transactions)
            .HasForeignKey(transaction => transaction.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Establishing a connection between one Category and many Transactions.
        modelBuilder.Entity<Transaction>()
            .HasOne(transaction => transaction.Category)
            .WithMany(category => category.Transactions)
            .HasForeignKey(transaction => transaction.CategoryId);

        List<User> users = new List<User>()
        {
            new User() { Id = 1,
                Name = "Artem",
                Surname="Babenko",
                Email="artem@gmail.com",
                Password="123123123"}
        };

        List<Category> categories = new List<Category>()
        {
            new Category() { Id = 1, Name = "Housing", IsIncome = false},
            new Category() { Id = 2, Name = "Transportation", IsIncome = false},
            new Category() { Id = 3, Name = "Food", IsIncome = false},
            new Category() { Id = 4, Name = "Insurance", IsIncome = false},
            new Category() { Id = 5, Name = "Utilities", IsIncome = false},
            new Category() { Id = 6, Name = "Debt Payments", IsIncome = false},
            new Category() { Id = 7, Name = "Personal Care", IsIncome = false},
            new Category() { Id = 8, Name = "Entertainment", IsIncome = false},
            new Category() { Id = 9, Name = "Education", IsIncome = false},
            new Category() { Id = 10, Name = "Health", IsIncome = false},
            new Category() { Id = 11, Name = "Salary", IsIncome = true},
            new Category() { Id = 12, Name = "Business", IsIncome = true},
            new Category() { Id = 13, Name = "Investment", IsIncome = true},
            new Category() { Id = 14, Name = "Rental", IsIncome = true},
            new Category() { Id = 15, Name = "Side Job", IsIncome = true},
            new Category() { Id = 16, Name = "Pension", IsIncome = true},
            new Category() { Id = 17, Name = "Social Security", IsIncome = true},
            new Category() { Id = 18, Name = "Alimony", IsIncome = true},
            new Category() { Id = 19, Name = "Gifts", IsIncome = true},
            new Category() { Id = 20, Name = "Other Income", IsIncome = true}
        };

        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Category>().HasData(categories);
    }
}
