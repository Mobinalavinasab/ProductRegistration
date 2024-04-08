using System.Security.Claims;
using Application;
using Application.AppDbContext;
using Application.JWT;
using Data.Repositories;
using Infrastructure.Entity.Role;
using Infrastructure.Entity.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Office_management.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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

builder.Services.AddAutoMapper(typeof(ICustomMapping));

builder.Services.AddScoped<IJWTRepository, JWTRepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.AccessDeniedPath = "/";
    options.LoginPath = "/";
})
.AddJwtBearer(options =>
 {
     var secretkey = System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]);
     var encryptionkey = System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:EncryptKey"]);

     var validationParameters = new TokenValidationParameters
     {
         ClockSkew = TimeSpan.Zero, 
         RequireSignedTokens = true,

         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(secretkey),
         RequireExpirationTime = true,
         ValidateLifetime = true,

         ValidateAudience = true, 
         ValidAudience = builder.Configuration["JWT:Audience"],

         ValidateIssuer = true, 
         ValidIssuer = builder.Configuration["JWT:Issuer"],

         TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
     };
     options.RequireHttpsMetadata = false;
     options.SaveToken = true;
     options.TokenValidationParameters = validationParameters;
     options.Events = new JwtBearerEvents
     {
         OnAuthenticationFailed = context =>
         {
             if (context.Exception != null)
                 throw new Exception("Authentication failed.");
             return Task.CompletedTask;
         },
         OnTokenValidated = async context =>
         {
             var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
             var userRepository = context.HttpContext.RequestServices.GetRequiredService<IRepository<User>>();
             var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
             if (claimsIdentity.Claims?.Any() != true)
                 context.Fail("This token has no claims.");
             var securityStamp = claimsIdentity?.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType)?.Value;

             var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
             if (validatedUser == null)
                 context.Fail("Token security stamp is not valid.");
         },
         OnChallenge = context =>
         {
             if (context.Request.Path.ToString().Contains("report"))
             {
                 context.Response.Redirect("/login");
                 context.HandleResponse();
                 return Task.CompletedTask;
             }
             if (context.AuthenticateFailure != null)
                 throw new Exception("Authenticate failure.");
             return Task.CompletedTask;
         },
         OnMessageReceived = context =>
         {
             return Task.CompletedTask;
         }
     };
 });

builder.Services.Configure<DataProtectionTokenProviderOptions>(option => option.TokenLifespan = TimeSpan.FromHours(10));

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigoration>();




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