using NLog.Web;
using Scalar.AspNetCore;
using SimpleManualDispatcher.Server.API.Repository;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

// DB接続文字列
builder.Services.Configure<DbConnectionOptions>(
    builder.Configuration.GetSection("ConnectionStrings"));

// ログ設定
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Dependency Injection


builder.Services.Configure<AspnetCore>(
    builder.Configuration.GetSection("ScalarOptions"));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
