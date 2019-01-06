namespace dodidec.Tests.TestClasses
{
    public class Bosh : IMessage
    {
        private readonly IMessage _innerItem;

        public Bosh(IMessage innerItem)
        {
            _innerItem = innerItem;
        }

        public string GetMessage()
        {
            return $"{_innerItem.GetMessage()} {GetType().Name}";
        }
    }
}