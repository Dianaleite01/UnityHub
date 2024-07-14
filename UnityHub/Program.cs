using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using UnityHub.Data;
using UnityHub.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UnityHubContextConnection")
                      ?? throw new InvalidOperationException("Connection string 'UnityHubContextConnection' not found.");

// Configuração do DbContext com SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddMemoryCache();


// Configuração dos serviços de identidade
builder.Services.AddDefaultIdentity<Utilizadores>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>() // Adiciona suporte a roles
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configuração de cookies de autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Utilizadores/Login"; // Rota para a página de login
});

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Adição dos controladores com views
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 64; // Define a profundidade máxima desejada
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Adicionado para autenticação
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapControllers();
app.Run();
