using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using ProcessQuestDataContracts.Models.Stages;

namespace ProcessQuestDataContracts.JsonHelpers
{
    public static class ProcessQuestJsonSerializerOptions
    {
        public static JsonSerializerOptions SetProcessQuestJsonSerializerOptions(this JsonSerializerOptions options)
        {
            //политика камелКейса
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //десериализатор Stage
            options.Converters.Add(new ProcessStageJsonConverterHelper<StageProcess>());
            //сравнение полей без учета регистра
            options.PropertyNameCaseInsensitive = true;
            options.AllowTrailingCommas = true;

            return options;
        }
    }
}
