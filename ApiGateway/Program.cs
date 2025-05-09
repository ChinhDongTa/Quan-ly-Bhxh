using DataServices.Data;
using DataServices.Entities.Human;
using DongTa.BaseDapper;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var bhxhDbConnectionString = builder.Configuration.GetConnectionString("Employee") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<BhxhDbContext>(options =>
    options.UseSqlServer(bhxhDbConnectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

//Add Dapper connection
builder.Services.AddScoped<IConnectionStringService, ConnectionStringService>(sp => new ConnectionStringService(builder.Configuration));
builder.Services.AddScoped<IGenericDapper, GenericDapper>();

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApiUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BhxhDbContext>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() // Cho phép tất cả nguồn gốc
                  .AllowAnyMethod() // Cho phép tất cả phương thức HTTP
                  .AllowAnyHeader(); // Cho phép tất cả header
        });
});
var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.MapIdentityApi<ApiUser>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();