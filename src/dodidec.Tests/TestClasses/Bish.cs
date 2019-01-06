namespace dodidec.Tests.TestClasses
{
    public class Bish : IMessage
    {
        public string GetMessage()
        {
            return GetType().Name;
        }
    }
}