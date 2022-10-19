using System.Text;
using praca_inzynierska_backend.Data;
using praca_inzynierska_backend.Repositories.AccountRepository;
using praca_inzynierska_backend.Repositories.ArtworksRepository;
using praca_inzynierska_backend.Repositories.FilesRepository;
using praca_inzynierska_backend.Services.AccountService;
using praca_inzynierska_backend.Services.ArtworksService;
using praca_inzynierska_backend.Services.UploadService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(configuration["ConnectionStrings:db"]);
});

// builder.Services.AddDbContextPool<AppDbContext>(opt =>
// {
//     opt.UseSqlServer(configuration["ConnectionStrings:db"]);

// });

// builder.Services.AddDbContextFactory<AppDbContext>(opt =>
// {
//     opt.UseSqlServer(configuration["ConnectionStrings:db"]);
// });

builder.Services
    .AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:Secret"])
            )
        };
    });

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IArtworksService, ArtworksService>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IFilesRepository, FilesRepository>();
builder.Services.AddScoped<IArtworksRepository, ArtworksRepository>();
builder.Services.AddCors(
    o =>
        o.AddPolicy(
            "CorsPolicy",
            builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }
        )
);
var app = builder.Build();
app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
