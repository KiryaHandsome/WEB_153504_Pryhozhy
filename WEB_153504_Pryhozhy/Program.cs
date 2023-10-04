using Microsoft.Extensions.DependencyInjection;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Models;
using WEB_153504_Pryhozhy.Services;
using WEB_153504_Pryhozhy.Services.CategoryService;
using WEB_153504_Pryhozhy.Services.PizzaService;
using WEB_153504_Pryhozhy.TagHelpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
UriData? uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IPizzaService, ApiPizzaService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
builder.Services.AddScoped(SessionCart.GetCart);
builder.Services.AddScoped<PagerTagHelper>();

// add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
        options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
    });

builder.Services.AddRazorPages();

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
    .UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app
    .MapRazorPages()
    .RequireAuthorization();

app.Run();
