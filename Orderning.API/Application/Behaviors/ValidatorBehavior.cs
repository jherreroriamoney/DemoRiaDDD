﻿using FluentValidation;
using MediatR;
using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Commands;
using Ordering.Domain.Exceptions;

namespace Ordering.API.Application.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var typeName = request.GetGenericTypeName();
            /* asi se hace en el flujo normal en mi app de MicroserviciosDDD
            _logger.LogInformation("----- Validating command {CommandType}", typeName);

            var validationResult = await _validator.ValidateAsync((CreateOrderCommand)request);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.ToString());
            }
            */

            var failures = _validators
           .Select(v => v.Validate(request))
           .SelectMany(result => result.Errors)
           .Where(error => error != null)
           .ToList();

            if (failures.Any())
            {
                _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

                throw new OrderingDomainException(
                    $"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures, true));
            }

            return await next();
        }
    }
}