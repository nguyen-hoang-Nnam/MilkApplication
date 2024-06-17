using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MilkApplication.BLL.Service;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.Mapper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository;
using MilkApplication.DAL.Repository.IRepositpry;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using MilkApplication.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add DB
builder.Services.AddDbContext<AppDbContext>(option =>
{
    var connectionString = builder.Configuration.GetConnectionString("DB");
    option.UseSqlServer(connectionString);
});

// Add Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Config Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
});

// Add Authentication and JwtBeaer
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });
// Inject app Dependency Injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOriginRepository, OriginRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVouchersRepository, VouchersRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOriginService, OriginService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVouchersService, VouchersService>();
builder.Services.AddScoped<ILocationService, LocationService>();

// Add JwtHelper
builder.Services.AddScoped<JwtHelper>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Bear
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: Bearer Generated-JWT-Token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        }, new string[] { }
                    }
                });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowSpecificOrigin");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    if (!app.Environment.IsDevelopment())
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API");
        c.RoutePrefix = string.Empty;
    }
});
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
