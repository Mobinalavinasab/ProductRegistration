using Application.AppDbContext;
using Data.Repositories;
using Infrastructure.Entity.Role;
using Infrastructure.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddDbContext<MyAppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ProductRegistration"));
});

builder.Services.AddIdentity<User, Role>(identityOptions =>
    {
        identityOptions.Password.RequireDigit = false;
        identityOptions.Password.RequiredLength = 0;
        identityOptions.Password.RequireNonAlphanumeric = false;
        identityOptions.Password.RequireUppercase = false;
        identityOptions.Password.RequireLowercase = false;

        //UserName Settings
        identityOptions.User.RequireUniqueEmail = false;
    }).AddEntityFrameworkStores<MyAppDbContext>()
    .AddDefaultTokenProviders();



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