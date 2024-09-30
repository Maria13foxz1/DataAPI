using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Messanger.Models;
using Messanger.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Messanger.Controllers;
public class CountryProbability
{
    public string CountryId { get; set; }
    public double Probability { get; set; }
}
public class DbController : Controller
{
    private readonly MessangerDbContext _context;
    private readonly HttpClient _httpClient;

    public DbController(MessangerDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task AddUser(UserModel user)
    {
        var isUserExist = await _context.users.FirstOrDefaultAsync(u => u.user_name == user.user_name) != null;

        if (!isUserExist)
        {
            var allUsers = await _context.users.OrderBy(o => o.user_id).ToListAsync();
            UserModel newUser = new UserModel();
            newUser.user_name = user.user_name;
            newUser.user_email = user.user_email;
            newUser.password = user.password;
            newUser.user_id = allUsers[allUsers.Count - 1].user_id + 1;
            await _context.users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ActionResult<UserModel>> GetUserData(string userName)
    {
        var user = await _context.users.FirstOrDefaultAsync(v => v.user_name == userName);
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    public List<UserModel> GetAllUsers()
    {
        return _context.users.ToList();
    }

    public async Task<IActionResult> OpenDate()
    {
        var allData = await _context.NameInfo.ToListAsync();
        return View("~/Views/Home/Open.cshtml", allData);
    }

    public async Task<IActionResult> SaveNameInfo(string chatID)
    {
        Console.WriteLine(chatID);
        var genderApiUrl = $"https://api.genderize.io/?name={chatID}";
        var ageApiUrl = $"https://api.agify.io/?name={chatID}";
        var nationalityApiUrl = $"https://api.nationalize.io/?name={chatID}";
        var allMessages = await _context.NameInfo.OrderBy(m => m.Id).ToListAsync();
        
        var genderResponse = await _httpClient.GetStringAsync(genderApiUrl);
        var ageResponse = await _httpClient.GetStringAsync(ageApiUrl);
        var nationalityResponse = await _httpClient.GetStringAsync(nationalityApiUrl);

        var genderData = JsonConvert.DeserializeObject<dynamic>(genderResponse);
        var ageData = JsonConvert.DeserializeObject<dynamic>(ageResponse);
        var nationalityData = JsonConvert.DeserializeObject<dynamic>(nationalityResponse);
        var id_count = allMessages[allMessages.Count - 1].Id + 1;
        Console.WriteLine(nationalityData+"nationalityData");
        var nameInfo = new NameInfoModel()
        {
            Id = id_count,
            Name = chatID,
            Gender = genderData.gender,
            GenderProbability = (float)genderData.probability,
            Age = (int)ageData.age,
            NationalityData = JsonConvert.SerializeObject(nationalityData.country)
        };
        _context.NameInfo.Add(nameInfo);
        await _context.SaveChangesAsync();
        return View("Index", nameInfo);
    }
} 