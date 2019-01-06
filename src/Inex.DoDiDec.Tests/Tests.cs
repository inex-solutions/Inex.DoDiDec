using Inex.DoDiDec.Tests.TestClasses;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Inex.DoDiDec.Tests
{
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