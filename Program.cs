using API.Extensions;
using API.Helpers;
using API.Middleware;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<MaskanContext>(opt =>
//{
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});


//var x = AbkCrypto.Crypto.EncryptString(builder.Configuration.GetConnectionString("FxConnection"));
//var y = AbkCrypto.Crypto.EncryptString(builder.Configuration.GetConnectionString("ICConnection"));
//var z = AbkCrypto.Crypto.EncryptString(builder.Configuration.GetConnectionString("IdentityConnection"));

//var xx = AbkCrypto.Crypto.DecryptString(builder.Configuration.GetConnectionString("FxConnection"));
//var yy = AbkCrypto.Crypto.DecryptString(builder.Configuration.GetConnectionString("ICConnection"));
//var zz = AbkCrypto.Crypto.DecryptString(builder.Configuration.GetConnectionString("IdentityConnection"));


//Console.WriteLine("FxConnection:" + AbkCrypto.Crypto.DecryptString(builder.Configuration.GetConnectionString("FxConnection")));
//Console.WriteLine("ICConnection:" + AbkCrypto.Crypto.DecryptString(builder.Configuration.GetConnectionString("ICConnection")));
//Console.WriteLine("IdentityConnection:" + AbkCrypto.Crypto.DecryptString(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddDbContext<FXRATEContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("FxConnection"), builder => builder.EnableRetryOnFailure(5 , TimeSpan.FromSeconds(10), null ));
});
builder.Services.AddDbContext<ICContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ICConnection"), builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
});
builder.Services.AddDbContext<AppIdentityDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"), builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
});

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//                .AddEntityFrameworkStores<AppIdentityDbContext>()
//                .AddDefaultTokenProviders();

builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

//builder.Services.AddMvc(Option => Option.Filters.Add(typeof(GlobalFilter)));


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var identityContext = services.GetRequiredService<AppIdentityDbContext>();

    await identityContext.Database.MigrateAsync();
    await AppIdentityDbContextSeed.SeedRolesAsync(roleManager);
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);


}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred during migration");
}
app.Run();

