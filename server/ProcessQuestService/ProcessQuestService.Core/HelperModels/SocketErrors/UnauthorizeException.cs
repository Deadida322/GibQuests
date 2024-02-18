namespace ProcessQuestService.Core.HelperModels.SocketErrors
{
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException() : base("Не авторизирован!")
        { }
        public UnauthorizeException(string message) : base(message)
        { }
    }
}
