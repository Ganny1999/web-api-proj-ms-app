using auth_api_ms.Data;
using auth_api_ms.DomainModel.Interfaces;
using auth_api_ms.DomainModel.Models;
using auth_api_ms.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
});
//Add Identity
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =   JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options=>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        ValidIssuer = builder.Configuration["jwt:Issuer"],
        ValidAudience = builder.Configuration["jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"]))
    };
});

//
builder.Services.AddScoped<IAuthService, AuthServices>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
