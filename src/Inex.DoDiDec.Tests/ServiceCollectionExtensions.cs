using System;
using Microsoft.Extensions.DependencyInjection;

namespace Inex.DoDiDec.Tests
{
    public static class ServiceCollectionExtensions
    {
        public static void AddByType<TService, TImplementation>(this ServiceCollection serviceCollection,
            Lifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            switch (lifetime)
            {
                case Lifetime.Transient:
                {
                    serviceCollection.AddTransient<TService, TImplementation>();
                    break;
                }
                case Lifetime.Scoped:
                {
                    serviceCollection.AddScoped<TService, TImplementation>();
                    break;
                }
                case Lifetime.Singleton:
                {
                    serviceCollection.AddSingleton<TService, TImplementation>();
                    break;
                }
                default:
                {
                    throw new NotSupportedException($"Lifetime not supported: {lifetime}");
                }
            }
        }

        public static void AddByFactory<TService>(this ServiceCollection serviceCollection, Lifetime lifetime,
            Func<IServiceProvider, TService> factory)
            where TService : class
        {
            switch (lifetime)
            {
                case Lifetime.Transient:
                {
                    serviceCollection.AddTransient(factory);
                    break;
                }
                case Lifetime.Scoped:
                {
                    serviceCollection.AddScoped(factory);
                    break;
                }
                case Lifetime.Singleton:
                {
                    serviceCollection.AddSingleton(factory);
                    break;
                }
                default:
                {
                    throw new NotSupportedException($"Lifetime not supported: {lifetime}");
                }
            }
        }
    }
}