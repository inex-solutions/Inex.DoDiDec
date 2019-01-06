using System;
using Microsoft.Extensions.DependencyInjection;

namespace Inex.DoDiDec
{
    public static class ServiceDescriptorExtensions
    {
        public static ServiceDescriptor CloneWithNewServiceType(this ServiceDescriptor descriptorToClone,
            Type newServiceType)
        {
            if (descriptorToClone.ImplementationType != null)
                return new ServiceDescriptor(newServiceType, descriptorToClone.ImplementationType,
                    descriptorToClone.Lifetime);

            if (descriptorToClone.ImplementationFactory != null)
                return new ServiceDescriptor(newServiceType, descriptorToClone.ImplementationFactory,
                    descriptorToClone.Lifetime);

            if (descriptorToClone.ImplementationInstance != null)
                return new ServiceDescriptor(newServiceType, descriptorToClone.ImplementationInstance);

            throw new NotSupportedException(
                "Cannot clone a service descriptor which has none of ImplementationType, ImplementationFactory or ImplementationInstance set: " +
                descriptorToClone);
        }
    }
}