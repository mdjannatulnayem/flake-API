// <<flake weather service>>

using Serilog;
using Microsoft.EntityFrameworkCore;
using flake_API;
using flake_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .WriteTo.File("Logs/apilogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationDbContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddControllers(option => {
    //option.ReturnHttpNotAcceptable = true;
    }).AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddSwaggerGen();

// Configure CORS policy
builder.Services.AddCors(p => p.AddPolicy("corspolicy",build => { 
    build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

// <<flake weather service>>