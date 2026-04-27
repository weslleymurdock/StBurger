using StBurger.Infrastructure.Decorators;

namespace StBurger.Composition.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureStBurgerServices(IConfiguration configuration)
        {
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    var http = context.HttpContext;

                    context.ProblemDetails.Extensions["traceId"] = http.TraceIdentifier;
                    context.ProblemDetails.Instance =
                        $"{http.Request.Method} {http.Request.Path}";
                };
            });

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
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
            services.AddScoped<IOrderReadOnlyRepository, OrderReadOnlyRepository>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IOrderService, OrderService>();
            services.Scan(scan => scan
    .FromAssembliesOf(typeof(IMenuService), typeof(IOrderService))
    .AddClasses(classes => classes
        .Where(t => t.Name.EndsWith("Service")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
            services.Decorate<IMenuService, MenuServiceLoggingDecorator>();
            services.Decorate<IOrderService, OrderServiceLoggingDecorator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Application.Menu.Commands.CreateMenuItemCommand>());
            services.AddValidatorsFromAssemblyContaining<Application.Menu.Validators.CreateMenuItemCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            return services;
        }
    }
}
