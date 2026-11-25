using Microsoft.EntityFrameworkCore;
using HomeCook.Api.EntityFramework;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Services;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Stripe;
using CloudinaryDotNet;
using Hangfire;
using Hangfire.PostgreSql;
using HomeCook.Api.Constants;

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
    options.UseNpgsql(connectionString, o => o.UseNetTopologySuite()));

builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddUserManager<AppUserManager>();

builder.Services.AddHttpClient<IPostcodeService, PostcodeService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApis:PostcodeApi"]!);
});

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

    // API: return status codes instead of redirecting to login / access denied pages
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// Re-validate the cookie against the database every 5 minutes so role changes apply quickly
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
    options.ValidationInterval = TimeSpan.FromMinutes(5));


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

// Background jobs (Stripe sync, image cleanup, email retries)
// Jobs are stored in PostgreSQL in their own "hangfire" schema, which is created on first start.
builder.Services.AddHangfire(configuration => configuration
      .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
      .UseSimpleAssemblyNameTypeSerializer()
      .UseRecommendedSerializerSettings()
      .UsePostgreSqlStorage(options =>
        options.UseNpgsqlConnection(connectionString),
        // Check for new jobs once a minute instead of every 15 seconds, so an idle database can sleep
        new PostgreSqlStorageOptions { QueuePollInterval = TimeSpan.FromMinutes(1) }));

// Runs the jobs in the background inside this API process
builder.Services.AddHangfireServer();

builder.Services.AddScoped<ICategoryReposity, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFoodSearchService, FoodSearchService>();
builder.Services.AddScoped<IFoodSearchRepository, FoodSearchRepositoy>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
builder.Services.AddScoped<IUserAddressService, UserAddressService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


var app = builder.Build();

// Create roles and promote configured admin emails
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(scope.ServiceProvider, app.Configuration);
}

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

// Job dashboard, admins only (uses the same cookie login and roles as the API)
app.MapHangfireDashboard("/hangfire", new DashboardOptions
{
    // Turn off Hangfire's own check (local requests only) and let ASP.NET Core decide
    Authorization = []
})
.RequireAuthorization(policy => policy.RequireRole(Roles.Admin));

app.MapControllers();

app.Run();
