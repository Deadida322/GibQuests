using GenerateQuestsService.DataContracts.Models.Stages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenerateQuestsService.DataContracts.JsonHelpers
{
    public static class QuestJsonSerializerOptions
    {
        public static JsonSerializerOptions SetQuestJsonSerializerOptions(this JsonSerializerOptions options)
        {
            //политика камелКейса
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //десериализатор Stage
            options.Converters.Add(new StageJsonConverterHelper<Stage>());
            //десериализатор строковых энамов
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            //сравнение полей без учета регистра
            options.PropertyNameCaseInsensitive = true;

            return options;
        }
    }
}
