namespace ProcessQuestService.Core.HelperModels.SocketErrors
{
    public class InternalServerException : Exception
    {
        public InternalServerException() : base("Неизвестная ошибка!")
        { }
        public InternalServerException(string message) : base(message)
        { }
    }
}
