using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Inex.DoDiDec
{
    public static class DecoratorExtensions
    {
        public static IServiceCollection AddDecorator<TService>(this IServiceCollection services,
            Func<TService, TService> decoratorFactory)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (decoratorFactory == null) throw new ArgumentNullException(nameof(decoratorFactory));

            var serviceType = typeof(TService);

            var matchingServiceDescriptors = services.Where(d => d.ServiceType == serviceType).ToArray();

            if (matchingServiceDescriptors.Length == 0)
                throw new DecoratorSetupException(
                    $"Decorator setup failed: service type not registered: {serviceType.FullName}");

            if (matchingServiceDescriptors.Length > 1)
                throw new DecoratorSetupException(
                    $"Decorator setup failed: multiple registrations for service type: {serviceType.FullName}");

            var decoratedOriginalServiceType = typeof(DecoratedType<>).MakeGenericType(typeof(TService));

            if (services.Any(d => d.ServiceType == decoratedOriginalServiceType))
                throw new DecoratorSetupException(
                    "Decorator setup failed: there is already a decorator registered for this type. " +
                    $"You cannot register a decorator for an already decorated entry: {serviceType.FullName}");

            var originalServiceDescriptor = matchingServiceDescriptors[0];
            var decoratedOriginalServiceDescriptor =
                originalServiceDescriptor.CloneWithNewServiceType(decoratedOriginalServiceType);

            services.Add(decoratedOriginalServiceDescriptor);

            var decoratorDescriptor = new ServiceDescriptor(
                serviceType,
                sp => decoratorFactory.Invoke((TService) sp.GetService(decoratedOriginalServiceType)),
                originalServiceDescriptor.Lifetime);

            services.Replace(decoratorDescriptor);

            return services;
        }
    }
}