using Listovki.Controllers.Services;
using Listovki.Data;
using Listovki.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration.GetValue<string>("JwtKey");
var jwtIssuer = builder.Configuration.GetValue<string>("JwtIssuer");
var SvelteSpecificOrigins = "_svelteSpecificOrigins";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddCors(options => {
    options.AddPolicy(name: SvelteSpecificOrigins,
        policy => {
            policy.WithOrigins("http://localhost:5555").AllowAnyHeader().AllowCredentials().AllowAnyMethod();
        });
});

builder.Services.AddDefaultIdentity<AppUser>().AddDefaultTokenProviders().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options => {
        options.RequireHttpsMetadata = false; // For development, set to true in production
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtIssuer, // Optional: Set if you have a specific issuer
            ValidAudience = jwtIssuer, // Optional: Set if you have a specific audience
            ClockSkew = TimeSpan.FromDays(1) // Reduce the default clock skew to immediate token expiry
        };
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
} else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(SvelteSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
