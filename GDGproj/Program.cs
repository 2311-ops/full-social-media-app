using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GDGproj.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("Server=DESKTOP-9I2RJ27;Database=AppDB;Trusted_Connection=True;TrustServerCertificate=True;");
});
var jwtkey = builder.Configuration["Jwt:Key"];

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();