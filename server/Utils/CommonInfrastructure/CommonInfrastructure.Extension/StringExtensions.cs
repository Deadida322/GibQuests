namespace CommonInfrastructure.Extension
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNull(this string str)
        {
            return str == null;
        }
    }

}
