using FlowDeskCo.Application.Interfaces;
using FlowDeskCo.Infrastructure.Persistence;
using FlowDeskCo.Infrastructure.Repositories;
using FlowDeskCo.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestateCo.Domain.Entities.CoreEntities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Register Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // If you added JWT authentication, tell Swagger how to send the token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
});

builder.Services.AddDbContext<AppDbContext>((sp, options) => {

    var tenantProvider = sp.GetRequiredService<ITenantProvider>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseSeeding((context, _) =>
    {
        if (!context.Set<Role>().Any())
        {
            context.Set<Role>().AddRange(DbSeedData.roles);
        }
        if (!context.Set<Client>().Any())
        {
            context.Set<Client>().AddRange(DbSeedData.clients);
        }
        if (!context.Set<User>().Any())
        {
            context.Set<User>().AddRange(DbSeedData.users);
        }
        if (!context.Set<Document>().Any())
        {
            context.Set<Document>().AddRange(DbSeedData.documents);
        }
        if (!context.Set<SharedLink>().Any())
        {
            context.Set<SharedLink>().AddRange(DbSeedData.sharedLinks);
        }
        if (!context.Set<SharedLink>().Any())
        {
            context.Set<SharedLink>().AddRange(DbSeedData.sharedLinks);
        }
        context.SaveChanges();
    });
    });


//Authentication Middleware
builder.Services.AddIdentity<User,Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});

builder.Services.AddScoped<JwtTokenService>();

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
