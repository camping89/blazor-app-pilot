using System.Text.Json.Serialization;
using BlazorApp.Application.Repositories;
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

builder.Services.AddSingleton<GenerateService>();
builder.Services.AddSingleton<DeviationService>();
builder.Services.AddTransient<IBaseRepository<Shift>, BaseRepository<Shift>>();
builder.Services.AddTransient<IShiftRepository, ShiftRepository>();
builder.Services.AddTransient<IBaseRepository<Deviation>, BaseRepository<Deviation>>();
builder.Services.AddTransient<IDeviationRepository, DeviationRepository>();
builder.Services.AddTransient<IBaseRepository<Client>, BaseRepository<Client>>();
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IBaseRepository<Employee>, BaseRepository<Employee>>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IRandomRepository, RandomRepository>();


var app = builder.Build();

await (app.Services.GetService<IRandomRepository>())?.RandomData()!;



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