using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ================================
// ADD SERVICES
// ================================

// MVC
builder.Services.AddControllersWithViews();

// SESSION
builder.Services.AddSession( );

// HTTP CONTEXT ACCESSOR
builder.Services.AddHttpContextAccessor();

// ================================
// BUILD APP
// ================================
var app = builder.Build();

// ================================
// MIDDLEWARE
// ================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// SESSION MUST BE BEFORE AUTH
app.UseSession();

app.UseAuthorization();

// ================================
// ROUTING
// ================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Login}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
