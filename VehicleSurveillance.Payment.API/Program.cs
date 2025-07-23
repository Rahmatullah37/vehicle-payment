using Microsoft.Extensions.DependencyInjection;
using VehicleSurveillance.Data.Infrastructure;
using VehicleSurveillance.Payment.API.Mapping;
using AutoMapper;
using VehicleSurveillance.Payment.API.Infrastructure.Migration;

//using AutoMapper.Extensions.Microsoft.DependencyInjection;
using DataRegistry = VehicleSurveillance.Data.Infrastructure.Registory;
using VehicleSurveillance.Data.Repositories;
using VehicleSurveillance.Services.Implementations;
var builder = WebApplication.CreateBuilder(args);

// run migrations
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnectionString");
DbMigrationRunner.Run(connectionString);


// Add services to the container.
DataRegistry.AddServicesToContainer(builder.Services);
builder.Services.AddScoped<PackagesServices>();
builder.Services.AddScoped<AccessFeeTransactionService>();
builder.Services.AddScoped<TarifService>();
builder.Services.AddScoped<FixedTarifService>();
builder.Services.AddScoped<PaymentModeService>();
builder.Services.AddScoped<TarifTypeService>();
builder.Services.AddScoped<VehicleTypeService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



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

app.UseAuthorization();

app.MapControllers();

app.Run();
