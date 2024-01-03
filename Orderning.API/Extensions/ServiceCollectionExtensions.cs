using MediatR;
using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Commands;
using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Queries;
using Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure.Services;
using System.Reflection;

namespace Ordening.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Se registra MediatR
            //services.AddMediatR(typeof(CreateOrderDraftCommandHandler).GetTypeInfo().Assembly);
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

                //cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                //cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
                //cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });

            // Registro de servicio que consume la infraestructura
            //services.AddTransient<IIdentityService, IdentityService>();

            // Registro de queries
            //Es lo mismo que esto "services.AddScoped<IUserQueries, UserQueries>();" pero para pasarle parametros al constructor
            //services.AddScoped<IOrderQueries>(sp => new OrderQueries(configuration.GetConnectionString("SQLServer") ?? "Invalid configuration.GetConnectionString(\"SQLServer\")"));

            return services;
        }
    }
}
