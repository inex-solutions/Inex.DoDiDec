using System;

namespace dodidec
{
    public class DecoratorSetupException : InvalidOperationException
    {
        public DecoratorSetupException(string message) : base(message)
        {
        }
    }
}