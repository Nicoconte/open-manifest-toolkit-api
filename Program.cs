using Microsoft.EntityFrameworkCore;
using Open.ManifestToolkit.API.Data;
using Open.ManifestToolkit.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ManifestYamlService>();
builder.Services.AddTransient<ManifestService>();
builder.Services.AddTransient<SettingService>();
builder.Services.AddTransient<GitService>();

builder.Services.AddCors(b =>
{
    b.AddDefaultPolicy(opt =>
    {
        opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration["ConnectionString"]);
    opt.UseLazyLoadingProxies();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
