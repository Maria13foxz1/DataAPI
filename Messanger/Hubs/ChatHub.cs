using System.Diagnostics.Tracing;
using Messanger.Controllers;
using Messanger.Models;
using Messanger.Models.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Hubs;

public class ChatHub:Hub
{
    private readonly MessangerDbContext _context;
    private DbController db;

    public ChatHub(MessangerDbContext context)
    {
        _context = context;
        db  = new DbController(_context);
    }

    public async void SendMessage(int chatId, string user, string message)
    {
        var timestamp = DateTime.Now;
        Console.Write(user+" "+message);
        await Clients.Group(chatId.ToString()).SendAsync("RecieveMessage", user, message, timestamp);

    } 
    public async Task JoinChat(int chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    
    public async Task<List<MessageModel>> GetChatMessages(int chatId)
    {
        var chat = await _context.chatmodel.FirstOrDefaultAsync(c => c.chat_id == chatId);
        if (chat == null)
        {
            return new List<MessageModel>();
        }
        var messages = await _context.messagemodel
            .Where(m => chat.messages.Contains(m.message_id))
            .ToListAsync();
        Console.WriteLine($"Message: {messages[0].content}");
        return messages;
    }
    public async Task<string> GetUserNameById(int userId)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.user_id == userId);
        if (user != null)
        {
            return user.user_name;
        }
        return string.Empty;
    }
}