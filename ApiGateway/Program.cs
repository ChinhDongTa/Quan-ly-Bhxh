using DataServices.Data;
using DataServices.Entities.Human;
using DongTa.BaseDapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var bhxhDbConnectionString = builder.Configuration.GetConnectionString("Employee") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<BhxhDbContext>(options =>
    options.UseSqlServer(bhxhDbConnectionString));

//Add Dapper connection
builder.Services.AddScoped<IConnectionStringService, ConnectionStringService>(sp => new ConnectionStringService(builder.Configuration));
builder.Services.AddScoped<IGenericDapper, GenericDapper>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm",
        policy =>
        {
            policy.WithOrigins("https://localhost:7293")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApiUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BhxhDbContext>();
//.AddDefaultTokenProviders()
//.AddDefaultIdentity<ApiUser>(options =>
//{
//    options.User.RequireUniqueEmail = true;
//    options.SignIn.RequireConfirmedAccount = false;
//    options.Password.RequireDigit = false;
//    options.Password.RequiredLength = 6;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;
//});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowBlazorWasm");
app.MapIdentityApi<ApiUser>();
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();