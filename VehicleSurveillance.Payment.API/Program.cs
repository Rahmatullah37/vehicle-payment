using Microsoft.Extensions.DependencyInjection;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.API.Mapping;
using AutoMapper;
using VisualSoft.Surveillance.Payment.API.Infrastructure.Migration;

//using AutoMapper.Extensions.Microsoft.DependencyInjection;
using DataRegistry = VisualSoft.Surveillance.Payment.Data.Infrastructure.Registory;
using VisualSoft.Surveillance.Payment.Data.Repositories;
using VisualSoft.Surveillance.Payment.Services.Implementations;
using VisualSoft.Surveillance.Payment.Domain.Configurations;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Application.Services;
using VisualSoft.Surveillance.Payment.Services.Interfaces;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); 
builder.Services.AddScoped<IServiceConfiguration, ServiceConfiguration>();


builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();
builder.Services.AddScoped<IDBMigrator, DbMigrationRunner>();
builder.Services.AddScoped<IUserIdentificationModel, UserIdentificationModel>();




// Add services to the container.
DataRegistry.AddServicesToContainer(builder.Services);
builder.Services.AddScoped<PackagesServices>();
builder.Services.AddScoped<AccessFeeTransactionService>();
builder.Services.AddScoped<TarifService>();
builder.Services.AddScoped<FixedTarifService>();
builder.Services.AddScoped<PaymentModeService>();
builder.Services.AddScoped<TarifTypeService>();
builder.Services.AddScoped<VehicleTypeService>();
builder.Services.AddScoped<VehiclePackageService>();
//builder.Services.AddScoped<VehicleAccountService>();
builder.Services.AddScoped<IVehicleAccountService, VehicleAccountService>();




builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// run migrations
var sp = builder.Services.BuildServiceProvider();
var dbmigrator = sp.GetService<IDBMigrator>();
dbmigrator.DBMigrate();


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
