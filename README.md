# DoDiDec

#### Dotnet Core Dependency Injection Decorator

This library allows a decorator to be registered with the .Net Core dependency injection container.

This means that when the service is requested from the container, instead of the originally registered implementation being returned, the original is decorated with the supplied decorator and this latter instance is returned.

The .AddDecorator call must be called on the ServiceCollection _after_ the type being decorated has been registered.
A given service type can only be decorated once using this method.
