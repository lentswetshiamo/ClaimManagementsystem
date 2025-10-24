using ClaimManagementsystem.Data;
using ClaimManagementsystem.Data.Repository;
using ClaimManagementsystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// DbContext
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();

// Application services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ClaimService>();
builder.Services.AddScoped<WorkflowService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DocumentService>();
builder.Services.AddScoped<AuditService>(); // depends on IAuditRepository

// Session & HttpContext
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LoginPage}/{id?}");

app.MapRazorPages();

app.Run();