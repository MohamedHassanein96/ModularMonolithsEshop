using Catalog.Data.Seed;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Shared.Behaviors;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Data.Seed;
using System.Reflection;

namespace Catalog;

public static class CatalogModule
{


    public static IServiceCollection AddCatalogModule(this IServiceCollection services , IConfiguration configuration)
    {

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        services.AddScoped<IDataSeeder, CatalogDataSeeder>();


        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<CatalogDbContext>((sp,options) => 
        { 
            options.UseNpgsql(connectionString);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });

        //services.AddMediatR(config =>
        //{
        //    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

        //    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        //    config.AddOpenBehavior(typeof (LoggingBehavior<,>));
        //});
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        //services.AddCarter(configurator :config =>
        //{
        //    var CatalogModules = typeof(CatalogModule).Assembly.GetTypes()
        //      .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

        //    config.WithModules(CatalogModules);
        //});

        


       return services;





    }



    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        //HTTP Request Pipeline Configurations

        app.UseMigration<CatalogDbContext>();
        return app; 
    }

}
