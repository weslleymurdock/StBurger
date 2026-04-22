using FluentValidation;
using StBurger.Infrastructure.Services;

namespace StBurger.Composition.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureStBurgerServices(IConfiguration configuration)
        {

            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
            });

            // Add services to the container.
            services.AddProblemDetails();
            services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi(options =>
            {
                options.AddScalarTransformers();
            });

            services.AddDbContext<StBurgerDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), dbOptions =>
                {
                    dbOptions.MigrationsAssembly(typeof(StBurgerDbContext).Assembly.FullName);
                    dbOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
                    dbOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), null);
                    dbOptions.CommandTimeout(45);
                });
            });
            services.AddScoped<IDbManager, DbManager>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Application.Menu.Commands.CreateMenuItemCommand>());
            services.AddValidatorsFromAssemblyContaining<Application.Menu.Validators.CreateMenuItemCommandValidator>();

            return services;
        }
    }
}
