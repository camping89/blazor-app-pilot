using BlazorApp.Application.Repositories;
using BlazorApp.Application.Services;
using BlazorApp.Share.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddSingleton<GenerateService>();
builder.Services.AddTransient<IBaseRepository<Shift>, BaseRepository<Shift>>();
builder.Services.AddTransient<IBaseRepository<Shift>, ShiftRepository>();
builder.Services.AddTransient<IBaseRepository<Deviation>, BaseRepository<Deviation>>();
builder.Services.AddTransient<IBaseRepository<Client>, BaseRepository<Client>>();
builder.Services.AddTransient<IBaseRepository<Employee>, BaseRepository<Employee>>();

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