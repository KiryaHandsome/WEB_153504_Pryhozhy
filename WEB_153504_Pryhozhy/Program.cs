using WEB_153504_Pryhozhy.Models;
using WEB_153504_Pryhozhy.Services.CategoryService;
using WEB_153504_Pryhozhy.Services.PizzaService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
UriData? uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IPizzaService, ApiPizzaService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services
    .AddScoped<ICategoryService, MemoryCategoryService>();
    //.AddScoped<IPizzaService, MemoryPizzaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
