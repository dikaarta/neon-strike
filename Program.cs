using Game.Web.Data;
using Game.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var home = Environment.GetEnvironmentVariable("HOME");

var dbPath = string.IsNullOrEmpty(home)
    ? Path.Combine(builder.Environment.ContentRootPath, "game.db")
    : Path.Combine(home, "data", "game.db");

Directory.CreateDirectory(
    Path.GetDirectoryName(dbPath)!);

builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<JwtTokenService>();

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Game API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using var scope = app.Services.CreateScope();

try
{
    var dbContext =
        scope.ServiceProvider.GetRequiredService<GameDbContext>();

    await dbContext.Database.MigrateAsync();

    await SeedData.InitializeAsync(dbContext);
}
catch (Exception ex)
{
    File.WriteAllText(
            Path.Combine(
                builder.Environment.ContentRootPath,
                "startup-error.txt"),
            ex.ToString());
    throw;
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
//}

app.UseHttpsRedirection();

app.UseDefaultFiles(); // index.html becomes default page
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
