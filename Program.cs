using System.Text;
using learn.Configs;
using learn.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register LearnContext to the API
builder.Services.AddDbContext<LearnContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("learnDatabase");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// add ConfigJwt to the service
builder.Services.Configure<ConfigJwt>(builder.Configuration.GetSection("Jwt"));

// add appsetting to the program
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json")
    .Build();


// add the JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidateLifetime = true,
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// add authentication middleware to the program
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
