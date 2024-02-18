namespace ProcessQuestService.Core.HelperModels.SocketErrors
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Невалидный запрос!")
        { }
        public BadRequestException(string message) : base(message)
        { }
    }
}
