using Carter;
using Serilog;
using Shared.Exceptions.Handlers;
using Shared.Extensions;
using Shared.Messaging.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));



builder.Services.AddCatalogModule(builder.Configuration)
                .AddBasketModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);





var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly =  typeof(BasketModule).Assembly;
var orderingAssembly = typeof(OrderingModule).Assembly;

builder.Services.AddCarterWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);
builder.Services.AddMedaitorWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services
    .AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, basketAssembly, orderingAssembly);


builder.Services.AddExceptionHandler<CustomExceptionHandler>();



var app = builder.Build();


app.MapCarter();
app.UseSerilogRequestLogging(); 
app.UseExceptionHandler(options => { });



app.UseCatalogModule()
    .UseBasketModule();

app.Run();
