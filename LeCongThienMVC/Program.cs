using LeCongThienMVC.Handlers;
using LeCongThienMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Service;
using System.Net.Http.Headers;

namespace LeCongThienMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            // Thêm HttpContextAccessor trước khi đăng ký các handler
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<JwtTokenHandler>();

            // Đăng ký các HttpClient với base address và handler
            var apiBaseAddress = "https://localhost:7050/api/";

            builder.Services.AddHttpClient<ICatergoryService, CatergoryService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            }).AddHttpMessageHandler<JwtTokenHandler>();

            builder.Services.AddHttpClient<INewsArticleService, NewsArticleService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            }).AddHttpMessageHandler<JwtTokenHandler>();

            builder.Services.AddHttpClient<ITagService, TagService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            }).AddHttpMessageHandler<JwtTokenHandler>();

            builder.Services.AddHttpClient<ISystemAccountService, SystemAccountService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            }).AddHttpMessageHandler<JwtTokenHandler>();


            builder.Services.AddHttpClient("ODataAPI", client => 
            {
                client.BaseAddress = new Uri("https://localhost:7097");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });


            // Cấu hình Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hết hạn cookie
                });

            builder.Services.AddAuthorization();

            // Cấu hình Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Nên bật trong production
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();//them
            app.UseAuthentication(); //them

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
