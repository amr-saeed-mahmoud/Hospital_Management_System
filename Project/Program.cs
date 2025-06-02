using Project.Extentions;
using UnitsOfWork;
using UnitsOfWork.Implemention;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InjectDbContext(builder.Configuration)
   .AddIdentityHandlersAndStores()
   .AddIdentityAuth(builder.Configuration)
   .AddSwaggerExplorer();

builder.Services.AddTransient<IMainUnit, MainUnit>();

var app = builder.Build();

app.ConfigureSwaggerExplorer()
   .AddIdentityAuthMiddlewares();

app.UseHttpsRedirection();

app.MapControllers();

await DataInitializer.UseAllDataSeed(app.Services.CreateScope().ServiceProvider);

app.Run();
