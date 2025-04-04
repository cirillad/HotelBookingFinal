using HotelBooking.DAL.Contexts;
using HotelBooking.DAL.Entities;
using HotelBooking.BLL.Services.AuthService;
using HotelBooking.DAL.Repositories.Users;
using HotelBooking_API.MapperProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HotelBooking.BLL.Services.BookingService;
using HotelBooking.DAL.Repositories.BookingRepos;
using HotelBooking.BLL.Services.EmailService;
using HotelBooking.BLL.Services.RoomService;
using HotelBooking.DAL.Repositories.RoomRepos;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ������������ Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// ������ Serilog � ����������
builder.Host.UseSerilog();

// ������ ���������
var logger = builder.Logging;
logger.ClearProviders();
logger.AddConsole();
logger.AddDebug();
logger.AddSerilog();

// ������������ PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NpgsqlLocal")));

// ������������ Identity
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// �������������� �� JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Log.Error("������� ��������������: {ErrorMessage}", context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Log.Information("����� ������������: {UserName}", context.Principal?.Identity?.Name);
                return Task.CompletedTask;
            }
        };
    });

// ������ AutoMapper
builder.Services.AddAutoMapper(typeof(BookingProfile));

// ��������� ���������� � ������
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();

// ������ ���������� �� Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ��������� ��� ����� ����������
Log.Information("������ ����������...");

// ����������� ���� �����
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

    await DatabaseSeeder.SeedAsync(userManager, roleManager);
}

// ������������ Swagger � ����� ��������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ������ Middleware ��� ������� ������� ����� UseRouting
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "�������� ������� ��� ������� ����������");
}
finally
{
    Log.CloseAndFlush();
}

