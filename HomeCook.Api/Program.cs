using Microsoft.EntityFrameworkCore;
using HomeCook.Api.EntityFramework;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Services;
using HomeCook.Api.Mappings;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Stripe;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("bearerAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {

            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}

        }, []
         }
    });
});

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>();

// TODO - Remember to SameSiteMode.None to SameSiteMode.Strict when deploying

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.LoginPath = "/api/login";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
    options.Cookie.Expiration = null;
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var cloudinaryUrl = builder.Configuration["CLOUDINARY_URL"];
if (string.IsNullOrWhiteSpace(cloudinaryUrl))
    throw new Exception("CLOUDINARY_URL is not set");

var cloudinary = new Cloudinary(cloudinaryUrl);
cloudinary.Api.Secure = true;
builder.Services.AddSingleton(cloudinary);

builder.Services.AddScoped<ICategoryReposity, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUpdateQuantityRepository, UpdateQuantityRepository>();
builder.Services.AddScoped<IUpdateQuantityService, UpdateQuantityService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFoodSearchService, FoodSearchService>();
builder.Services.AddScoped<IFoodSearchRepository, FoodSearchRepositoy>();

builder.Services.AddAutoMapper(typeof(AutoMapping));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.MapGroup("api").MapIdentityApi<User>();

app.MapControllers();

app.Run();
