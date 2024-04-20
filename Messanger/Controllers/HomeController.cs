using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Messanger.Models;
using Messanger.Models.Context;

namespace Messanger.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MessangerDbContext _context;

    public HomeController(ILogger<HomeController> logger, MessangerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Users()
    {
        var dbController = new DbController(_context);
        var users = dbController.GetAllUsers();
        return View(users);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Chat()
    {
        return View();
    }
}