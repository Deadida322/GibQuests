using System.Text.Json;

namespace CommonInfrastructure.Extension
{
    public static class ObjectsExtension
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public static string SafeSerialize<T1>(this T1 obj, string defaultValue = null)
            where T1 : class
        {
            if (obj == null)
                return defaultValue;

            try
            {
                var result = JsonSerializer.Serialize(obj, _options);

                return result;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T SafeDeserialize<T>(this string json, T defaultValue = null)
            where T : class
        {
            if (string.IsNullOrEmpty(json))
                return defaultValue;

            try
            {
                var result = JsonSerializer.Deserialize<T>(json, _options);
                return result;
            }
            catch
            {
                return defaultValue;
            }
        }
    }

}
