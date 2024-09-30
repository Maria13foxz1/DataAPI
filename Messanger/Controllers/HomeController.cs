using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Messanger.Models;
using Messanger.Models.Context;


namespace Messanger.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MessangerDbContext _context;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, MessangerDbContext? context, HttpClient httpClient)
    {
        _logger = logger;
        _context = context;
        _httpClient = httpClient;
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
        var dbController = new DbController(_context, _httpClient);
        
        var users = dbController.GetAllUsers();
        return View(users);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Open()
    {
        return View();
    }
}
