using GenerateQuestsService.DataContracts.JsonHelpers;
using GenerateQuestsService.DataContracts.Models.Stages;
using ProcessQuestDataContracts.JsonHelpers;
using ProcessQuestDataContracts.Models.Stages;
using ProcessQuestService.Core.InteractionWebSocketModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProcessQuestService.Core.Helpers
{
    public class QuestJsonSerializer
    {
        private JsonSerializerOptions _jsonSerializerOptions;
        public QuestJsonSerializer() {
            _jsonSerializerOptions = new JsonSerializerOptions().SetQuestJsonSerializerOptions();
            _jsonSerializerOptions.Converters.Add(new ProcessStageJsonConverterHelper<StageProcess>());
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
        }

        public byte[] SerializeAsByte<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                JsonSerializer.Serialize<T>(ms, obj, _jsonSerializerOptions);
                return ms.ToArray();
            }
        }

        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, _jsonSerializerOptions);
        }
    }
}
