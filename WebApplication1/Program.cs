using Microsoft.EntityFrameworkCore;
using WebApplication1.MovieRecData;
using WebApplication1.MovieData;
using WebApplication1.UserData;
using Npgsql;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Entitites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;


Env.Load();

var builder = WebApplication.CreateBuilder(args);

var movieConnectionString = builder.Configuration.GetConnectionString("MovieConnection");

var movieDataSourceBuilder = new NpgsqlDataSourceBuilder(movieConnectionString);
movieDataSourceBuilder.EnableDynamicJson();
var movieDataSource = movieDataSourceBuilder.Build();

builder.Services.AddDbContext<MDbContext>(options =>
{
    options.UseNpgsql(movieDataSource);
});

var userConnectionString = builder.Configuration.GetConnectionString("UserConnection");

var userDataSourceBuilder = new NpgsqlDataSourceBuilder(userConnectionString);
var userDataSource = userDataSourceBuilder.Build();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
        options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
        options.Scope.Add("email");
        options.SaveTokens = true;
    })
    .AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
    {
        options.AppId = builder.Configuration["FacebookKeys:AppId"];
        options.AppSecret = builder.Configuration["FacebookKeys:AppSecret"];
        options.Scope.Add("email");
        options.SaveTokens = true;
    });

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(userDataSource);
});

builder.Services.AddIdentity<User, IdentityRole>()
   .AddEntityFrameworkStores<UserDbContext>();

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var movieDbContext = scope.ServiceProvider.GetRequiredService<MDbContext>();
    movieDbContext.Database.Migrate();

    var userDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    userDbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowAll");

app.Run();