using BlazorAPI.Controller;
using BlazorAPI.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<CategoriesController>(); // Controller
builder.Services.AddDbContext<NewTestDateBaseContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("myconn"))); // ContextDB


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();
app.MapControllers();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
