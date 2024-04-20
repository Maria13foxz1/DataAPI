using Microsoft.EntityFrameworkCore;

namespace Messanger.Models.Context;

public class MessangerDbContext: DbContext
{
    public MessangerDbContext(DbContextOptions<MessangerDbContext> options) : base(options)
    { }

    public DbSet<ChatModel> chatmodel{get; set;}
    public DbSet<UserModel> users{get; set;}
    public DbSet<MessageModel> messagemodel{get; set;}
}