using EventLocator.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseUrls("http://0.0.0.0:10000"); // required for Render

// ? Add services BEFORE calling builder.Build()
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EventPlannerDb"));
builder.Services.AddSession();

var app = builder.Build();

// ? Seed data AFTER app is built
SeedData(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();              // ? Important for session use
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// ? Seed dummy data method
void SeedData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.EventPlanners.AddRange(
        new EventPlanner { Name = "Sunrise Events", Address = "Kochi", Latitude = 9.9312, Longitude = 76.2673 },
        new EventPlanner { Name = "Dream Decor", Address = "Trivandrum", Latitude = 8.5241, Longitude = 76.9366 },
        new EventPlanner { Name = "Elegant Moments", Address = "Calicut", Latitude = 11.2588, Longitude = 75.7804 },

        // ? Kollam-based planners
        new EventPlanner { Name = "Royal Weddings", Address = "Kollam Beach", Latitude = 8.8932, Longitude = 76.6141 },
        new EventPlanner { Name = "Kollam Celebrations", Address = "Chinnakada, Kollam", Latitude = 8.8853, Longitude = 76.5860 },
        new EventPlanner { Name = "Elite Events Kollam", Address = "Kadappakada", Latitude = 8.8940, Longitude = 76.6102 }
    );

    db.SaveChanges();
}
