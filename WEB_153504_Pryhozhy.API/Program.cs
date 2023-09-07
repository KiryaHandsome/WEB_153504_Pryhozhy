using Microsoft.EntityFrameworkCore;
using WEB_153504_Pryhozhy.API.Data;
using WEB_153504_Pryhozhy.API.Services.CategoryService;
using WEB_153504_Pryhozhy.API.Services.PizzaService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

DbInitializer.SeedData(app).GetAwaiter().GetResult();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
