using Messanger.Models;
using Messanger.Models.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Messanger.Controllers;

public class AuthorizationController:Controller
{
    private MessangerDbContext _context;
    private readonly HttpClient _httpClient;

    public AuthorizationController(MessangerDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }
    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterModel model)
    {
        var tempModel = new RegAndLogModel() { register = model };
        DbController dbController = new DbController(_context, _httpClient);
        LoginModel? login = new LoginModel(); 
        if (ModelState.IsValid)
        {
            var isUserExist = await _context.users.FirstOrDefaultAsync(u => u.user_email == model.email) == null;
            
            if (isUserExist)
            {
                var userToAdd = new UserModel()
                {
                    user_name = model.name,
                    user_email = model.email,
                    password = model.password,
                };
                await dbController.AddUser(userToAdd);
                login.email = model.email;
                login.password = model.password;
                await Login(login);
            }
            else
            {
                TempData["ErrorMessage2"] = "Користувач з таким email вже існує";
                return View("~/Views/Home/Index.cshtml", tempModel);
            }
        }
        else
        {
            TempData["ErrorMessage2"] = "Користувач з таким email вже існує";
            return View("~/Views/Home/Index.cshtml", tempModel);
        }

        return RedirectToAction("Users", "Home");
    }

    public async Task<IActionResult> Login(LoginModel model)
    {
        var tempModel = new RegAndLogModel() { login = model };
        var dbController = new DbController(_context, _httpClient);
        if (ModelState.IsValid)
        {
            var userDetails =
                _context.users.SingleOrDefault(m =>
                    m.user_email == model.email &&
                    m.password == model.password);
            if (userDetails == null)
            {
                TempData["ErrorMessage1"] = "Логін або пароль введені неправильно";
                return RedirectToAction("Index", "Home", tempModel);
            }

            HttpContext.Session.SetString("userName", userDetails.user_name);
            await _context.SaveChangesAsync();
        }
        else
        {
            TempData["ErrorMessage1"] = "Логін або пароль введені неправильно";
            return RedirectToAction("Index", "Home");
        }
        return RedirectToAction("Users", "Home");
    }
    
    // Повертає true якщо існує користувач із заданим ім'ям.
    public bool IsUserExist(string userName)
    {
        return _context.users.FirstOrDefault(v => v.user_name == userName) == null;
    }

    // Повертає true кщо існує користувач із заданою поштою.
    public bool IsEmailExist(string email)
    {
        return _context.users.FirstOrDefault(v => v.user_email == email) == null;
    }
}