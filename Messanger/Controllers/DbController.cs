using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Messanger.Models;
using Messanger.Hubs;
using Messanger.Models.Context;
using Microsoft.AspNetCore.Mvc;

namespace Messanger.Controllers;

public class DbController : Controller
{
    private readonly MessangerDbContext _context;

    public DbController(MessangerDbContext context)
    {
        _context = context;
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
            newUser.user_id = allUsers[allUsers.Count-1].user_id+1;
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

    public async Task<IActionResult> CreateChat()
    {
        var allChats = await _context.chatmodel.OrderBy(c => c.chat_id).ToListAsync();
        ChatModel chat = new ChatModel();
        chat.chat_id = allChats[allChats.Count - 1].chat_id + 1;
        chat.users = new List<int>();
        chat.messages = new List<int>();
        chat.users.Add(GetUserData(HttpContext.Session.GetString("userName")).Result.Value.user_id);
        await _context.chatmodel.AddAsync(chat);
        await _context.SaveChangesAsync();
        return View("~/Views/Home/Chat.cshtml", chat);
    }

    public async Task<IActionResult> ConnectToChat(int chatId)
    {
        var chat = _context.chatmodel.FirstOrDefault(c => c.chat_id == chatId);
        if (chat != null)
        {
            chat.users.Add(GetUserData(HttpContext.Session.GetString("userName")).Result.Value.user_id);
            await _context.SaveChangesAsync();
            return View("~/Views/Home/Chat.cshtml", chat);
        }
        return  View("Error");
    }

    [HttpPost]
    public async Task<string> AddMessage(int chatId, string user, string message)
    {
        Console.WriteLine(message+"1234567890");
        var contents = message;
        var allMessages = await _context.messagemodel.OrderBy(m => m.message_id).ToListAsync();
        Console.WriteLine(message+"1234567890");
        var messager = new MessageModel();
        Console.WriteLine(message+"1234567890");
        messager.message_id = allMessages[allMessages.Count - 1].message_id + 1;
        messager.message_time = DateTime.Now.ToUniversalTime();
        Console.WriteLine(message+"1234567890");
        var data = await GetUserData(user);
        messager.sender = data.Value.user_id;
        Console.WriteLine(contents+"1234567890");
        messager.content = contents;
        await _context.messagemodel.AddAsync(messager);
        Console.WriteLine(message+"1234567890");
        _context.chatmodel.FirstOrDefaultAsync(c => c.chat_id == chatId).Result.messages.Add(messager.message_id);
        await _context.SaveChangesAsync();
        return "OK";
    }
} 