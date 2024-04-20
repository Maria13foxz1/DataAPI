using Messanger.Hubs;
using Messanger.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore;
namespace Messanger;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSession();
        builder.Services.AddSignalR();
        
        builder.Services.AddDbContext<MessangerDbContext>(
            options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddSession(
            options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = false; // Виводити помилки валідації
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseCookiePolicy();
        app.UseStaticFiles();
        app.UseSession();
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        
        app.MapHub<ChatHub>("/Home/Chat/chat");
        app.UseRouting();

        app.UseAuthorization();
        
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}