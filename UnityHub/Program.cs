using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnityHub.Data;
using UnityHub.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UnityHubContextConnection")
                      ?? throw new InvalidOperationException("Connection string 'UnityHubContextConnection' not found.");

// Configuração do DbContext com SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configuração dos serviços de identidade
builder.Services.AddDefaultIdentity<Utilizadores>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()  // Adiciona suporte a roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Adição dos controladores com views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Adicionado para autenticação
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
