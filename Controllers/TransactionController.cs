using SpendWiseWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SpendWiseWebApp.Controllers;

// The TransactionController class is responsible for handling HTTP requests related to transactions.
public class TransactionController : Controller
{
    private readonly ApplicationDbContext db;
    public TransactionController(ApplicationDbContext db) => this.db = db;

    // HTTP GET method for rendering the OverviewPage.
    [HttpGet] [Authorize] [Route("/")]
    public IActionResult OverviewPage()
    {
        return View("/Views/Overview.cshtml");
    }

    // HTTP GET method for rendering the TransactionsPage.
    [HttpGet] [Authorize] [Route("/transactions")]
    public async Task<IActionResult> TransactionsPage()
    {
        // Retrieve transactions from the database, including related category information.
        var transactions = await db.Transactions
            .Include(t => t.Category)
            .ToListAsync();

        // Retrieve categories from the database and store them in the ViewBag for use in the view.
        ViewBag.Categories = await db.Categories.ToListAsync();
        return View("/Views/Transactions.cshtml", transactions);
    }

    // HTTP GET method for rendering the BudgetPage.
    [HttpGet] [Authorize] [Route("/budget")]
    public IActionResult BudgetPage()
    {
        return View("/Views/Budget.cshtml");
    }

    // HTTP GET method for rendering the CreatePage.
    [HttpGet] [Authorize] [Route("/create")]
    public async Task<IActionResult> CreatePage()
    {
        // Retrieve categories from the database and pass them as model data to the Create view.
        return View("/Views/Create.cshtml", await db.Categories.ToListAsync());
    }

    // HTTP GET method for rendering the EditPage.
    [HttpGet] [Authorize] [Route("/edit/{id:int}")]
    public async Task<IActionResult> EditPage(int id)
    {
        // Retrieve the transaction with the specified id, including related category information.
        var transaction = await db.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
        // If the transaction is not found, return a 404 Not Found response.
        if (transaction is null) return NotFound();

        // Retrieve categories from the database and pass them as model data to the Edit view.
        ViewBag.Categories = await db.Categories.ToListAsync();
        return View("/Views/Edit.cshtml", transaction);
    }

    // HTTP POST method for handling transaction editing.
    [HttpPost] [Authorize] [Route("/transactions/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        // Retrieve the transaction with the specified id from the database.
        var transaction = await db.Transactions.FindAsync(id);
        // If the transaction is not found, return a 404 Not Found response.
        if (transaction is null) return NotFound();
        // Retrieve form data from the request.
        var form = Request.Form;
        // Attempt to parse the categoryId from the form data.
        if (!int.TryParse(form["categoryId"].ToString(), out int categoryId)) 
            return RedirectToAction("EditPage", "Transaction", id);

        // Extract other form data.
        string? title = form["title"];
        float amount = int.Parse(form["amount"].ToString());
        bool isIncome = form["type"] == "income";

        // Update transaction properties with the new data.
        transaction.Title = title;
        transaction.Amount = amount;
        transaction.IsIncome = isIncome;
        transaction.CategoryId = categoryId;

        // Save changes to the database.
        await db.SaveChangesAsync();
        // Redirect to the TransactionsPage after successful editing.
        return RedirectToAction("TransactionsPage", "Transaction");
    }

    // HTTP GET method for handling transaction deletion.
    [HttpGet] [Authorize] [Route("/transactions/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        // Retrieve the transaction with the specified id from the database.
        var transaction = await db.Transactions.FindAsync(id);
        // If the transaction is not found, return a 404 Not Found response.
        if (transaction is null) return NotFound();
        // Remove the transaction from the database and save changes.
        db.Transactions.Remove(transaction);
        await db.SaveChangesAsync();
        // Redirect to the TransactionsPage after successful deletion.
        return RedirectToAction("TransactionsPage", "Transaction");
    }

    // HTTP POST method for handling transaction creation.
    [HttpPost] [Authorize] [Route("/transactions")]
    public async Task<IActionResult> Create()
    {
        // Retrieve form data from the request.
        var form = Request.Form;

        // Validate and parse the categoryId from the form data.
        if (!int.TryParse(form["categoryId"].ToString(), out int categoryId)) 
            return RedirectToAction("CreatePage", "Transaction");

        // Retrieve other form data.
        string? title = form["title"];
        float amount = int.Parse(form["amount"].ToString());
        bool isIncome = form["type"] == "income";
        // Retrieve the userId from the authenticated user's claims.
        int userId = int.Parse(User.FindFirstValue("Id") ?? "0");

        // Create a new Transaction object with the retrieved data.
        Transaction newTransaction = new Transaction()
        {
            Title = title,
            Amount = amount,
            IsIncome = isIncome,
            CategoryId = categoryId,
            UserId = userId
        };

        // Add the new transaction to the database and save changes.
        await db.AddAsync(newTransaction);
        await db.SaveChangesAsync();

        // Redirect to the CreatePage after successful creation.
        return RedirectToAction("CreatePage", "Transaction");
    }
}
