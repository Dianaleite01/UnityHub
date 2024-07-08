using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnityHub.Data;
using UnityHub.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UnityHubContextConnection")
                      ?? throw new InvalidOperationException("Connection string 'UnityHubContextConnection' not found.");

// Configura��o do DbContext com SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configura��o dos servi�os de identidade
builder.Services.AddDefaultIdentity<Utilizadores>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()  // Adiciona suporte a roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configura��o do CORS
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

// Adi��o dos controladores com views
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

var app = builder.Build();

// Configura��o do pipeline de requisi��o HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Adicionado para autentica��o
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapControllers();
app.Run();
