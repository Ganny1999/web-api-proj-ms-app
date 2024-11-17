using Microsoft.EntityFrameworkCore;
using order_api_ms.Data;
using order_api_ms.DomainModel.IServices;
using order_api_ms.Mapping;
using order_api_ms.Services;
using order_api_ms.Services.IExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add External services
builder.Services.AddHttpClient("Product", u=>u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
});

// Add service
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductService, ProductService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
