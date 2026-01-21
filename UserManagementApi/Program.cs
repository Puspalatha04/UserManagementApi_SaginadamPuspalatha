
using UserManagementAPI.Models;
using UserManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();




var users = new List<AppUser>
{
    new AppUser { Id = 0, Name = "System", Email = "system@test.com" },
    new AppUser { Id = 1, Name = "Alice", Email = "alice@test.com" },
    new AppUser { Id = 2, Name = "Bob", Email = "bob@test.com" }
};


// MIDDLEWARE PIPELINE

app.UseMiddleware<ErrorHandlingMiddleware>();    // 1️⃣ First
app.UseMiddleware<AuthenticationMiddleware>();  // 2️⃣ Second
app.UseMiddleware<LoggingMiddleware>();         // 3️⃣ Last


// CRUD Operations


// GET all users
app.MapGet("/users", () =>
{
    return Results.Ok(users);
});

// GET user by ID
app.MapGet("/users/{id:int}", (int id) =>
{
    if (id < 0)
        return Results.BadRequest("User ID cannot be negative.");

    var user = users.FirstOrDefault(u => u.Id == id);
    return user == null
        ? Results.NotFound("User not found.")
        : Results.Ok(user);
});

// POST add user
app.MapPost("/users", (AppUser user) =>
{
    if (user.Id < 0)
        return Results.BadRequest("User ID cannot be negative.");

    if (string.IsNullOrWhiteSpace(user.Name))
        return Results.BadRequest("Name cannot be empty.");

    if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains("@"))
        return Results.BadRequest("Invalid email format.");

    if (users.Any(u => u.Id == user.Id))
        return Results.BadRequest("User already exists.");

    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

// PUT update user
app.MapPut("/users/{id:int}", (int id, AppUser updatedUser) =>
{
    if (id < 0)
        return Results.BadRequest("User ID cannot be negative.");

    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound("User not found.");

    if (string.IsNullOrWhiteSpace(updatedUser.Name))
        return Results.BadRequest("Name cannot be empty.");

    if (string.IsNullOrWhiteSpace(updatedUser.Email) || !updatedUser.Email.Contains("@"))
        return Results.BadRequest("Invalid email format.");

    user.Name = updatedUser.Name;
    user.Email = updatedUser.Email;

    return Results.Ok(user);
});

// DELETE user
app.MapDelete("/users/{id:int}", (int id) =>
{
    if (id < 0)
        return Results.BadRequest("User ID cannot be negative.");

    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound("User not found.");

    users.Remove(user);
    return Results.NoContent();
});

app.Run();
