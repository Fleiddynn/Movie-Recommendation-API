using Microsoft.EntityFrameworkCore;
using WebApplication1.MovieRecData;
using WebApplication1.UserData;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using DotNetEnv;

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

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(userDataSource);
});

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

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowAll");

app.Run();