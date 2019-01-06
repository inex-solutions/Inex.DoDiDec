using System;
using Inex.DoDiDec.Tests.TestClasses;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

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

    public enum Lifetime
    {
        Transient,
        Scoped,
        Singleton
    }

    public class Tests
    {
        [Theory]
        [InlineData(Lifetime.Transient)]
        [InlineData(Lifetime.Scoped)]
        [InlineData(Lifetime.Singleton)]
        public void TypeRegistered_SingleDecorator_ShouldWork(Lifetime lifetime)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddByType<IMessage, Bish>(lifetime);
            serviceCollection.AddDecorator<IMessage>(inner => new Bash(inner));

            var container = serviceCollection.BuildServiceProvider();
            var instance = container.GetRequiredService<IMessage>();

            Assert.Equal($"{nameof(Bish)} {nameof(Bash)}", instance.GetMessage());
        }

        [Theory]
        [InlineData(Lifetime.Transient)]
        [InlineData(Lifetime.Scoped)]
        [InlineData(Lifetime.Singleton)]
        public void TypeRegistered_DoubleDecorator_ShouldThrow(Lifetime lifetime)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddByType<IMessage, Bish>(lifetime);
            serviceCollection.AddDecorator<IMessage>(inner => new Bash(inner));

            Assert.Throws<DecoratorSetupException>(() =>
                serviceCollection.AddDecorator<IMessage>(inner => new Bosh(inner)));
        }

        [Theory]
        [InlineData(Lifetime.Transient)]
        [InlineData(Lifetime.Scoped)]
        [InlineData(Lifetime.Singleton)]
        public void FactoryRegistered_SingleDecorator_ShouldWork(Lifetime lifetime)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddByFactory<IMessage>(lifetime, serviceProvider => new Bish());
            serviceCollection.AddDecorator<IMessage>(inner => new Bash(inner));

            var container = serviceCollection.BuildServiceProvider();
            var instance = container.GetRequiredService<IMessage>();

            Assert.Equal($"{nameof(Bish)} {nameof(Bash)}", instance.GetMessage());
        }

        [Theory]
        [InlineData(Lifetime.Transient)]
        [InlineData(Lifetime.Scoped)]
        [InlineData(Lifetime.Singleton)]
        public void FactoryRegistered_DoubleDecorator_ShouldThrow(Lifetime lifetime)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddByFactory<IMessage>(lifetime, serviceProvider => new Bish());
            serviceCollection.AddDecorator<IMessage>(inner => new Bash(inner));

            Assert.Throws<DecoratorSetupException>(() =>
                serviceCollection.AddDecorator<IMessage>(inner => new Bosh(inner)));
        }

        [Fact]
        public void InstanceRegistered_DoubleDecorator_ShouldThrow()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IMessage>(new Bish());
            serviceCollection.AddDecorator<IMessage>(inner => new Bash(inner));

            Assert.Throws<DecoratorSetupException>(() =>
                serviceCollection.AddDecorator<IMessage>(inner => new Bosh(inner)));
        }

        [Fact]
        public void InstanceRegistered_SingleDecorator_ShouldWork()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IMessage>(new Bish());
            serviceCollection.AddDecorator<IMessage>(inner => new Bash(inner));

            var container = serviceCollection.BuildServiceProvider();
            var instance = container.GetRequiredService<IMessage>();

            Assert.Equal($"{nameof(Bish)} {nameof(Bash)}", instance.GetMessage());
        }
    }
}