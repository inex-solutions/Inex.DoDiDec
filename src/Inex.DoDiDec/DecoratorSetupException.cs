using System;

namespace Inex.DoDiDec
{
    public class DecoratorSetupException : InvalidOperationException
    {
        public DecoratorSetupException(string message) : base(message)
        {
        }
    }
}