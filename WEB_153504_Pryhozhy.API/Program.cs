using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WEB_153504_Pryhozhy.API.Data;
using WEB_153504_Pryhozhy.API.Services.CategoryService;
using WEB_153504_Pryhozhy.API.Services.PizzaService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("Default"))
);
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
        {
            opt.Authority = builder
            .Configuration
            .GetSection("ISUri").Value;
            opt.TokenValidationParameters.ValidateAudience = false;
            opt.TokenValidationParameters.ValidTypes =
            new[] { "at+jwt" };
        });

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorWasmPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:7213")
             .AllowAnyMethod()
             .AllowAnyHeader();
    });
});

var app = builder.Build();

DbInitializer.SeedData(app).GetAwaiter().GetResult();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("BlazorWasmPolicy");
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
