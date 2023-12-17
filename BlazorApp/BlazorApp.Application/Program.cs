using System.Text.Json.Serialization;
using BlazorApp.Application.Caching;
using BlazorApp.Application.Repositories.Implementations;
using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Application.Services;
using BlazorApp.Share.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// builder.Services.AddMvc()
//     .AddJsonOptions(
//         options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//     );


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddSingleton<DeviationService>();
builder.Services.AddTransient<IBaseRepository<Shift>, BaseRepository<Shift>>();
builder.Services.AddTransient<IShiftRepository, ShiftRepository>();
builder.Services.AddTransient<IBaseRepository<Deviation>, BaseRepository<Deviation>>();
builder.Services.AddTransient<IDeviationRepository, DeviationRepository>();
builder.Services.AddTransient<IBaseRepository<Client>, BaseRepository<Client>>();
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IBaseRepository<Employee>, BaseRepository<Employee>>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IRandomDataProvider, RandomDataProvider>();


var app = builder.Build();

// random data and store to cache
await (app.Services.GetService<IRandomDataProvider>())?.Generate()!;

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