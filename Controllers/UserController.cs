using SpendWiseWebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SpendWiseWebApp.Controllers;

// The UserController class is responsible for handling HTTP requests related to user actions.
public class UserController : Controller
{
    private readonly ApplicationDbContext db;
    public UserController(ApplicationDbContext db) => this.db = db;

    // HTTP POST method for handling user login.
    [HttpPost] [Route("/user/login")]
    public async Task<IActionResult> Login()
    {
        // Extracting a data model from a request body.
        LoginModel? loginModel = await Request.ReadFromJsonAsync<LoginModel>();
        // Validating the payload received from the request.
        if (loginModel is null || loginModel.Email is null || loginModel.Password is null) 
            return BadRequest("Invalid payload");

        // User search in the database. 
        User? user = await db.Users.FirstOrDefaultAsync(u =>
            u.Email == loginModel.Email &&
            u.Password == loginModel.Password);
        // If the user is not found, return Unauthorized status.
        if (user is null) return Unauthorized();

        // If the user is found, create authentification cookies.
        var claims = new List<Claim>()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name ?? ""),
            new Claim(ClaimTypes.Surname, user.Surname ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        // Signing in the user by creating an authentication cookie.
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);

        return Ok();
    }

    // HTTP POST method for handling user registration.
    [HttpPost] [Route("/user/registration")]
    public async Task<IActionResult> Registration()
    {
        // Extracting a data model from a request body.
        var newUser = await Request.ReadFromJsonAsync<User>();
        if (newUser is null) return BadRequest();

        // Adding a new user to database.
        await db.Users.AddAsync(newUser);
        await db.SaveChangesAsync();

        // Creating authentification cookies.
        var claims = new List<Claim>()
        {
            new Claim("Id", newUser.Id.ToString()),
            new Claim(ClaimTypes.Name, newUser.Name ?? ""),
            new Claim(ClaimTypes.Surname, newUser.Surname ?? ""),
            new Claim(ClaimTypes.Email, newUser.Email ?? "")
        };

        // Signing in the user by creating an authentication cookie.
        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);

        return Ok();
    }

    // HTTP GET method for handling user logout.
    [HttpGet] [Route("/user/logout")]
    public async Task<IActionResult> Logout()
    {
        // Signing out the current user by removing authentication cookies.
        await HttpContext.SignOutAsync();
        // Redirecting to the login page after successful logout.
        return RedirectToAction("LoginPage", "User");
    }

    // HTTP GET method for accessing the user settings page.
    [HttpGet] [Authorize] [Route("/settings")]
    public IActionResult SettingsPage()
    {
        return View("/Views/Settings.cshtml");
    }

    // HTTP GET method for accessing the login page.
    [HttpGet] [Route("/login")]
    public IActionResult LoginPage()
    {
        return File("/html/login.html", "text/html");
    }

    // HTTP GET method for accessing the user profile page.
    [HttpGet] [Authorize] [Route("/profile")]
    public IActionResult ProfilePage()
    {
        return View("/Views/Profile.cshtml");
    }
}

public record LoginModel(string Email, string Password);