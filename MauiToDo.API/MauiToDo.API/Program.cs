//using MauiToDo.API.Data;
//using MauiToDo.API.DTO;
//using MauiToDo.API.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.AspNetCore.Authorization;

//using System.Security.Claims;
//using System.IdentityModel.Tokens.Jwt;



//var builder = WebApplication.CreateBuilder(args);

//// SQL Server ba�lant�s�
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Identity
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<AppDbContext>();

//// Authentication (JWT vs. sonradan eklenecek)
//builder.Services.AddAuthentication();

//builder.Services.AddAuthorization();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


//// Identity servisleri al
//builder.Services.AddScoped<UserManager<ApplicationUser>>();
//builder.Services.AddScoped<SignInManager<ApplicationUser>>();

//// JWT Authentication
//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings["Issuer"],
//            ValidAudience = jwtSettings["Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
//        };
//    });

//var app = builder.Build();

//// ?? Kullan�c� Kayd�
//app.MapPost("/api/auth/register", async (Register dto, UserManager<ApplicationUser> userManager) =>
//{
//    var user = new ApplicationUser
//    {
//        UserName = dto.Username,
//        Email = dto.Email
//    };

//    var result = await userManager.CreateAsync(user, dto.Password);
//    Console.WriteLine($"Kay�t ba�ar�l� m�? {result.Succeeded}");
//    foreach (var error in result.Errors)
//    {
//        Console.WriteLine($"Hata: {error.Description}");
//    }
//    return result.Succeeded ? Results.Ok("Kay�t ba�ar�l�.") : Results.BadRequest(result.Errors);
//});


//app.MapPost("/api/auth/login", async (Login dto, UserManager<ApplicationUser> userManager, IConfiguration config) =>
//{
//    // Kullan�c� ad�n� kontrol et
//    var user = await userManager.FindByNameAsync(dto.Username);
//    if (user == null)
//    {
//        Console.WriteLine($"Kullan�c� bulunamad�: {dto.Username}");
//        return Results.Unauthorized();
//    }

//    // �ifre kontrol�
//    var passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
//    if (!passwordValid)
//    {
//        Console.WriteLine($"�ifre yanl��: {dto.Username}");
//        return Results.Unauthorized();
//    }

//    Console.WriteLine($"Giri� ba�ar�l�: {dto.Username}");

//    var claims = new[]
//    {
//        new Claim(ClaimTypes.Name, user.UserName),
//        new Claim(ClaimTypes.NameIdentifier, user.Id)
//    };

//    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]));
//    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//    var token = new JwtSecurityToken(
//        issuer: config["JwtSettings:Issuer"],
//        audience: config["JwtSettings:Audience"],
//        claims: claims,
//        expires: DateTime.UtcNow.AddHours(1),
//        signingCredentials: creds
//    );

//    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
//});

////// ?? Giri� ve Token olu�turma
////app.MapPost("/api/auth/login", async (Login dto, UserManager<ApplicationUser> userManager, IConfiguration config) =>
////{

////    var user = await userManager.FindByNameAsync(dto.Username);
////    Console.WriteLine(user != null ? "Kullan�c� bulundu" : "Kullan�c� yok");

////    bool isPasswordValid = await userManager.CheckPasswordAsync(user, dto.Password);
////    Console.WriteLine($"�ifre ge�erli mi? {isPasswordValid}");


////    if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
////        return Results.Unauthorized();

////    var claims = new[]
////    {
////        new Claim(ClaimTypes.Name, user.UserName),
////        new Claim(ClaimTypes.NameIdentifier, user.Id)
////    };

////    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]));
////    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
////    var token = new JwtSecurityToken(
////        issuer: config["JwtSettings:Issuer"],
////        audience: config["JwtSettings:Audience"],
////        claims: claims,
////        expires: DateTime.UtcNow.AddHours(1),
////        signingCredentials: creds
////    );

////    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
////});
//// ?? T�m g�revleri getir (sadece giri� yapm�� kullan�c�ya ait)
//app.MapGet("/api/todo", [Authorize] async (HttpContext http, AppDbContext db) =>
//{
//    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//    if (userId == null) return Results.Unauthorized();

//    var todos = await db.ToDoItems
//        .Where(t => t.UserId == userId)
//        .ToListAsync();

//    return Results.Ok(todos);
//});

//// ? G�rev ekle
//app.MapPost("/api/todo", [Authorize] async (HttpContext http, AppDbContext db, ToDoItem item) =>
//{
//    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//    if (userId == null) return Results.Unauthorized();

//    item.UserId = userId;
//    item.CreatedAt = DateTime.UtcNow;

//    db.ToDoItems.Add(item);
//    await db.SaveChangesAsync();

//    return Results.Created($"/api/todo/{item.Id}", item);
//});

//// ?? G�rev g�ncelle
//app.MapPut("/api/todo/{id}", [Authorize] async (HttpContext http, int id, AppDbContext db, ToDoItem updatedItem) =>
//{
//    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//    var existing = await db.ToDoItems.FindAsync(id);

//    if (existing == null || existing.UserId != userId)
//        return Results.NotFound();

//    existing.Title = updatedItem.Title;
//    existing.Description = updatedItem.Description;
//    existing.IsCompleted = updatedItem.IsCompleted;
//    existing.DueDate = updatedItem.DueDate;
//    existing.UpdatedAt = DateTime.UtcNow;

//    await db.SaveChangesAsync();

//    return Results.Ok(existing);
//});

//// ? G�rev sil
//app.MapDelete("/api/todo/{id}", [Authorize] async (HttpContext http, int id, AppDbContext db) =>
//{
//    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//    var item = await db.ToDoItems.FindAsync(id);

//    if (item == null || item.UserId != userId)
//        return Results.NotFound();

//    db.ToDoItems.Remove(item);
//    await db.SaveChangesAsync();

//    return Results.NoContent();
//});
//app.MapPost("/api/auth/change-password", [Authorize] async (
//    HttpContext http,
//    UserManager<ApplicationUser> userManager,
//    IConfiguration config,
//    ChangePassword dto) =>
//{
//    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//    if (userId == null) return Results.Unauthorized();

//    var user = await userManager.FindByIdAsync(userId);
//    if (user == null) return Results.Unauthorized();

//    var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

//    return result.Succeeded ? Results.Ok("�ifre de�i�tirildi.") : Results.BadRequest(result.Errors);
//});

//app.UseSwagger();
//app.UseSwaggerUI();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapGet("/", () => "MauiToDo API �al���yor!");

//app.Run();

using MauiToDo.API.Data;
using MauiToDo.API.DTO;
using MauiToDo.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;



var builder = WebApplication.CreateBuilder(args);

// SQL Server ba�lant�s�
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Authentication (JWT vs. sonradan eklenecek)
builder.Services.AddAuthentication();

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Identity servisleri al
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

var app = builder.Build();

// ?? Kullan�c� Kayd�
app.MapPost("/api/auth/register", async (Register dto, UserManager<ApplicationUser> userManager) =>
{
    var user = new ApplicationUser
    {
        UserName = dto.Username,
        Email = dto.Email
    };

    var result = await userManager.CreateAsync(user, dto.Password);
    Console.WriteLine($"Kay�t ba�ar�l� m�? {result.Succeeded}");
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"Hata: {error.Description}");
    }
    return result.Succeeded ? Results.Ok("Kay�t ba�ar�l�.") : Results.BadRequest(result.Errors);
});


app.MapPost("/api/auth/login", async (Login dto, UserManager<ApplicationUser> userManager, IConfiguration config) =>
{
    // Kullan�c� ad�n� kontrol et
    var user = await userManager.FindByNameAsync(dto.Username);
    if (user == null)
    {
        Console.WriteLine($"Kullan�c� bulunamad�: {dto.Username}");
        return Results.Unauthorized();
    }

    // �ifre kontrol�
    var passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
    if (!passwordValid)
    {
        Console.WriteLine($"�ifre yanl��: {dto.Username}");
        return Results.Unauthorized();
    }

    Console.WriteLine($"Giri� ba�ar�l�: {dto.Username}");

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: config["JwtSettings:Issuer"],
        audience: config["JwtSettings:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds
    );

    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
});

// ?? T�m g�revleri getir (sadece giri� yapm�� kullan�c�ya ait)
app.MapGet("/api/todo", [Authorize] async (HttpContext http, AppDbContext db) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userId == null) return Results.Unauthorized();

    var todos = await db.ToDoItems
        .Where(t => t.UserId == userId)
        .ToListAsync();

    return Results.Ok(todos);
});

// ? G�rev ekle - DEBUG VERSION
app.MapPost("/api/todo", [Authorize] async (HttpContext http, AppDbContext db, ToDoItem item) =>
{
    Console.WriteLine("=== POST /api/todo DEBUG ===");
    Console.WriteLine($"User authenticated: {http.User.Identity?.IsAuthenticated}");
    Console.WriteLine($"Title: {item.Title}");
    Console.WriteLine($"Description: {item.Description}");

    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    Console.WriteLine($"User ID from token: {userId}");

    if (userId == null)
    {
        Console.WriteLine("HATA: UserId null - Unauthorized");
        return Results.Unauthorized();
    }

    try
    {
        item.UserId = userId;
        item.CreatedAt = DateTime.UtcNow;

        Console.WriteLine($"Eklenen item - UserId: {item.UserId}, Title: {item.Title}");
        db.ToDoItems.Add(item);
        await db.SaveChangesAsync();

        Console.WriteLine($"? G�rev ba�ar�yla eklendi - ID: {item.Id}");
        return Results.Created($"/api/todo/{item.Id}", item);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"? HATA: {ex.Message}");
        Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        return Results.BadRequest(new { error = ex.Message });
    }
});

// ?? G�rev g�ncelle
app.MapPut("/api/todo/{id}", [Authorize] async (HttpContext http, int id, AppDbContext db, ToDoItem updatedItem) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var existing = await db.ToDoItems.FindAsync(id);

    if (existing == null || existing.UserId != userId)
        return Results.NotFound();

    existing.Title = updatedItem.Title;
    existing.Description = updatedItem.Description;
    existing.IsCompleted = updatedItem.IsCompleted;
    existing.DueDate = updatedItem.DueDate;
    existing.UpdatedAt = DateTime.UtcNow;

    await db.SaveChangesAsync();

    return Results.Ok(existing);
});

// ? G�rev sil
app.MapDelete("/api/todo/{id}", [Authorize] async (HttpContext http, int id, AppDbContext db) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var item = await db.ToDoItems.FindAsync(id);

    if (item == null || item.UserId != userId)
        return Results.NotFound();

    db.ToDoItems.Remove(item);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPost("/api/auth/change-password", [Authorize] async (
    HttpContext http,
    UserManager<ApplicationUser> userManager,
    IConfiguration config,
    ChangePassword dto) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userId == null) return Results.Unauthorized();

    var user = await userManager.FindByIdAsync(userId);
    if (user == null) return Results.Unauthorized();

    var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

    return result.Succeeded ? Results.Ok("�ifre de�i�tirildi.") : Results.BadRequest(result.Errors);
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "MauiToDo API �al���yor!");

app.Run();