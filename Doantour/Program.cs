
using Doantour.Controllers;
using Doantour.Extention;
using Doantour.Models;
using Doantour.Repository;
using Doantour.Service;
using Doantour.Services;
using Doantour.Vnpay;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddScoped<BookingReportService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

var jwtConfig = builder.Configuration.GetSection("JwtConfig");
var secretKeyString = jwtConfig["SecretKey"];
var issuer = jwtConfig["Issuer"];
var audience = jwtConfig["Audience"];
var tokenExpiryInDays = Convert.ToDouble(jwtConfig["TokenExpiryInDays"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<Hachutravelcontext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("HachutourDB")));

builder.Services.AddAuthorization();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<Hachutravelcontext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(BaseController<,>));
builder.Services.AddScoped<RepositoryFactory>();
builder.Services.AddScoped<ServiceFactory>();

// Thêm dịch vụ cho VnPay
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.Configure<VnPaySettings>(builder.Configuration.GetSection("Vnpay"));
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowAnyOrigin();
        });
});


builder.Services.AddHangfire((sp, config) =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HachutourDB"));
});
builder.Services.AddHangfireServer();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 0;
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeedData.InitializeAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Hangfire Dashboard"
});

//app.ConfigureCustomExceptionMiddleware();

app.Run();

