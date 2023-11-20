using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("SalesWebMvcContext"),
                     new MySqlServerVersion(new Version(8, 0, 28)),
                     builder => builder.MigrationsAssembly("SalesWebMvc"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Criar um escopo temporário e resolver o serviço de seeding dentro dele
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var seedingService = serviceProvider.GetRequiredService<SeedingService>();
    seedingService.Seed();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
