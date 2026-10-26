
using ProductLab.Components;
using ProductLab.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Blazor services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Read the Supabase connection string
var connectionString =
    builder.Configuration.GetConnectionString("superbase")
    ?? throw new InvalidOperationException(
        "Connection string 'superbase' was not found.");

// 3. Register EF Core with PostgreSQL
builder.Services.AddDbContextFactory<AppDbContext>(
    options => options.UseNpgsql(connectionString));

// 4. Register ProductService
builder.Services.AddScoped<ProductService>();

// 5. Build the application AFTER registering services
var app = builder.Build();

// 6. Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Error", createScopeForErrors: true);

    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 7. Run the application
app.Run();
