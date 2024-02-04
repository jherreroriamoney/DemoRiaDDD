using eShop.Ordering.API.Application.Validations;
using FluentValidation;
using MediatR;
using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Commands;
using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Queries;
using Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure.Services;
using Ordering.API.Application.Behaviors;

namespace Ordening.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Se registra MediatR

            // Register the command validators for the validator behavior (validators based on FluentValidation library)
            services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

                //TODO: Añadir los Behavior de MediatR
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
                //cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            }); 

            // Registro de servicio que consume la infraestructura
            services.AddHttpContextAccessor();
            services.AddTransient<IIdentityService, IdentityService>();

            // Registro de queries
            //Es lo mismo que esto "services.AddScoped<IUserQueries, UserQueries>();" pero para pasarle parametros al constructor
            services.AddScoped<IOrderQueries>(sp => new OrderQueries(configuration.GetConnectionString("DefaultConnection") ?? "Invalid configuration.GetConnectionString(\"SQLServer\")"));

            return services;
        }
    }
}
