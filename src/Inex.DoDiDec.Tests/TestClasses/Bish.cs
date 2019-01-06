namespace Inex.DoDiDec.Tests.TestClasses
{
    public class Bish : IMessage
    {
        public string GetMessage()
        {
            return GetType().Name;
        }
    }
}