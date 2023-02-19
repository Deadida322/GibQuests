namespace CommonInfrastructure.Extension
{
    public static class ExceptionExtensions
    {
        public static List<string> ToExceptionDetails(this Exception ex)
        {
            var result = new List<string>();
            if (ex == null) return result;

            result.Add($"Message : {ex.Message}");
            result.Add($"Inner exception : {ex.StackTrace ?? "empty"}");
            return result;
        }
    }
}
