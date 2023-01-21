using AppEnvironment;
using Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var appSettings = configuration.Get<AppSettings>();


builder.Services.AddScoped<AppSettings>();
builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(appSettings.ConnectionStrings.DefaultConnection, ServerVersion.AutoDetect(appSettings.ConnectionStrings.DefaultConnection));
});
// Add services to the container.

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
