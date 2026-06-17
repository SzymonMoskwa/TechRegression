using Microsoft.EntityFrameworkCore;
using TechRegression.Data;
using TechRegression.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TechRegression.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // wylogowanie admina po 30 min bezczynnoœci
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- AUTOMATYCZNA INICJALIZACJA BAZY (SEED DATA) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();

        if (!context.Categories.Any())
        {
            var defaultCategory = new Category { Name = "Hardware" };
            context.Categories.Add(defaultCategory);
            context.SaveChanges();

            if (!context.Articles.Any())
            {
                context.Articles.Add(new Article
                {
                    Title = "Witaj w TechRegression",
                    Content = "To jest automatycznie wygenerowany artyku³ testowy systemu. Jeœli to widzisz, baza danych zosta³a zainicjalizowana poprawnie.",
                    CategoryId = defaultCategory.Id,
                    ImagePath = "",
                    CreatedAt = DateTime.Now
                });
                context.SaveChanges();
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Wyst¹pi³ b³¹d podczas inicjalizacji (seeding) bazy danych.");
    }
}

app.Run();
