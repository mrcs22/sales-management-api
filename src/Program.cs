using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using tech_test_payment_api.Data;
using tech_test_payment_api.Repositories;
using tech_test_payment_api.Services;
using tech_test_payment_api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("DbConnectionString")));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


builder.Services.AddTransient<ISaleRepository, SaleRepository>();
builder.Services.AddTransient<ISaleService, SaleService>();
builder.Services.AddTransient<ISellerRepository, SellerRepository>();
builder.Services.AddTransient<ISellerService, SellerService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<SaleServiceExceptionMiddleware>();

app.Run();
