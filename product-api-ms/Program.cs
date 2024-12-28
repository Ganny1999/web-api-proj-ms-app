using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using product_api_ms.Data;
using product_api_ms.DomainModel.Interfaces;
using product_api_ms.Filters;
using product_api_ms.Mapping;
using product_api_ms.Services;
using product_api_ms.Services.IExternalServices;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
});

// Versioning : Working Code - if no version specified code will run on version 1.

builder.Services.AddApiVersioning(options=>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

});

//Auth Service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateActor = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ValidIssuer = builder.Configuration["jwt:Issuer"],
            ValidAudience = builder.Configuration["jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("jwt:Key").Value))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMIN",policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("USER",policy => policy.RequireRole("USER","ADMIN"));
});

// Dependeacy injection life cycle
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IRatingService,RatingService>();

// Add External services
builder.Services.AddHttpClient("Rating", u=>u.BaseAddress = new Uri(builder.Configuration["ServiceUrlss:Rating"]));


// Filter registration
builder.Services.AddScoped<CustomAuthenticationFilter>();
    builder.Services.AddScoped<CustomResourceFilter>();
    builder.Services.AddScoped<CustomExceptionFilter>();
    builder.Services.AddScoped<CustomActionFilter>();
    builder.Services.AddScoped<CustomResultFilter>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configure the HTTP request pipeline.
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
