using AppEnvironment;
using Audit.Core;
using Authenticaion.JWT;
using Authentication.Encryption;
using Business.AuthBusiness;
using Business.CartBusiness;
using Business.CategoryBusiness;
using Business.OperationClaimBusiness;
using Business.OrderProductBusiness;
using Business.ProductBusiness;
using Business.UserBusiness;
using Business.UserOperationClaimBusiness;
using Business.WalletBusiness;
using Context;
using Elasticsearch.Net;
using GenerateKey.OTP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.CartRepository;
using Repository.CategoryRepository;
using Repository.OperationClaimRepository;
using Repository.OrderProductRepository;
using Repository.OtpKeyRepository;
using Repository.ProductRepository;
using Repository.UserOperationClaimRepository;
using Repository.UserRepository;
using Repository.WalletRepository;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();
var appSettings = configuration.Get<AppSettings>();
builder.Services.AddScoped<AppSettings>();

builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
var otpKeyExpiration = configuration.GetSection("OtpKeyOptions").Get<OtpKeyOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IOperationClaimRepositoryService, OperationClaimRepositoryService>();
builder.Services.AddScoped<IUserRepositoryService, UserRepositoryService>();
builder.Services.AddScoped<IUserOperationClaimRepositoryService, UserOperationClaimRepositoryService>();
builder.Services.AddScoped<IOtpKeyRepositoryService, OtpKeyRepositoryService>();
builder.Services.AddScoped<IOperationClaimRepositoryService, OperationClaimRepositoryService>();
builder.Services.AddScoped<ICategoryRepositoryService, CategoryRepositoryService>();
builder.Services.AddScoped<IProductRepositoryService, ProductRepositoryService>();
builder.Services.AddScoped<ICartRepositoryService, CartRepositoryService>();
builder.Services.AddScoped<IOrderProductRepositoryService, OrderProductRepositoryService>();
builder.Services.AddScoped<IWalletRepositoryService, WalletRepositoryService>();


builder.Services.AddScoped<IOperationClaimBusinessService, OperationClaimBusinessService>();
builder.Services.AddScoped<IUserOperationClaimBusinessService, UserOperationClaimBusinessService>();
builder.Services.AddScoped<IAuthBusinessService, AuthBusinessService>();
builder.Services.AddScoped<IUserOperationClaimBusinessService, UserOperationClaimBusinessService>();
builder.Services.AddScoped<IUserBusinessService,UserBusinessService>();
builder.Services.AddScoped<ICategoryBusinessService, CategoryBusinessService>();
builder.Services.AddScoped<IProductBusinessService, ProductBusinessService>();
builder.Services.AddScoped<ICartBusinessService, CartBusinessService>();
builder.Services.AddScoped<IOrderProductBusinessService, OrderProductBusinessService>();
builder.Services.AddScoped<IWalletBusinessService, WalletBusinessService>();

builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<IOtpHelper, OtpHelper>();

Audit.Core.Configuration.Setup().
    UseMySql(config => config
        .ConnectionString(configuration.GetConnectionString("AuditConnection"))
        .TableName("event")
        .IdColumnName("event_id")
        .JsonColumnName("data"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(appSettings.ConnectionStrings.DefaultConnection, ServerVersion.AutoDetect(appSettings.ConnectionStrings.DefaultConnection));
});


builder.Services.AddControllers();
https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
