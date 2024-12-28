using Microsoft.EntityFrameworkCore;
using order_place_api_ms.Data;
using order_place_api_ms.DomainModel.IServices;
using order_place_api_ms.Services;
using order_place_api_ms.Services.IExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
}
);

builder.Services.AddHttpClient("Cart",u=>u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:OrderApi"]));
builder.Services.AddHttpClient("Product",u=>u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductApi"]));

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
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
