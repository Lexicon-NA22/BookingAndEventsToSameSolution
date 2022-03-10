using GymBooking.Web.Clients;
using GymBooking.Web.Data;
using GymBooking.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

//1
builder.Services.AddHttpClient();

//2
builder.Services.AddHttpClient("GymClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5000");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient("GymClient2", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001");
});

//builder.Services.AddHttpClient<BookingClient>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:5001");
//});

builder.Services.AddHttpClient<IBookingClient, BookingClient>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var config = services.GetRequiredService<IConfiguration>();

    //db.Database.EnsureDeleted();
    //db.Database.Migrate();

    //dotnet user-secrets set "AdminPW" "Löseord1!"
    var adminPW = config["AdminPW"];

    try
    {
        SeedData.InitAsync(db, services, adminPW).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {

        throw;
    }

}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=GymClasses}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
