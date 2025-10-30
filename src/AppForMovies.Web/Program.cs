using AppForMovies.Web;
using AppForMovies.Web.API;
using AppForMovies.Web.Components;
using AppForMovies.Web.Components.Account;
using AppForMovies.Web.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

string? connection2Database = Environment.GetEnvironmentVariable("DBConnection2Use");

// If we are using the Production Environment, then the AZURE DB should be used,
// otherwise the localdb or SQLite should be used
//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?source=recommendations&view=aspnetcore-7.0
switch (connection2Database) {
    case "SQLite":
        DbConnection _connection = new SqliteConnection("Filename=:memory:");
        //connection in case a persistent database is required
        //DbConnection _connection = new SqliteConnection("Data Source=Application.db;Cache=Shared");
        _connection.Open();
        builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(_connection));
        break;

    case "AzureSQL":
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
                       opt.UseSqlServer(Environment.GetEnvironmentVariable("AzureSQL")));

        break;
    default:
        //the localdb is used
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        break;
}

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

string? URI2API = builder.Configuration.GetValue(typeof(string), "AppForMovies_API") as string;

//the environment variable is defined in Properties\launchsettings.json
builder.Services.AddScoped<AppForMoviesAPIClient>(sp =>new AppForMoviesAPIClient(URI2API, new HttpClient()));

//adding an In-memory state container service
//https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management/?view=aspnetcore-8.0#in-memory-state-container-service
builder.Services.AddScoped<RentalStateContainer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();

