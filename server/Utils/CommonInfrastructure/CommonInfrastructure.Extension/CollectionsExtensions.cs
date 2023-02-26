namespace CommonInfrastructure.Extension
{
    public static class CollectionsExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                return true;

            if (!collection.Any())
                return true;

            return false;
        }

    }
}
