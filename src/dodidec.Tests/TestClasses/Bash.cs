namespace dodidec.Tests.TestClasses
{
    public class Bash : IMessage
    {
        private readonly IMessage _innerItem;

        public Bash(IMessage innerItem)
        {
            _innerItem = innerItem;
        }

        public string GetMessage()
        {
            return $"{_innerItem.GetMessage()} {GetType().Name}";
        }
    }
}