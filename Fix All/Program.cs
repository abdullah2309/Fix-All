using Fix_All.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<mydbcontext>(a=>a.UseSqlServer("Server=DESKTOP-KHBGNKV\\SQLEXPRESS;Database=Fix_All; Trusted_Connection=True; TrustServerCertificate=True"));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();   // ? Add this line
builder.Services.AddSession();               // already added for session


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
