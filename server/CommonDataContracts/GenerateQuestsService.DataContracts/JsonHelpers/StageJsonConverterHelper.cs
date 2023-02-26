using GenerateQuestsService.DataContracts.Enums;
using GenerateQuestsService.DataContracts.Models.Stages;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace GenerateQuestsService.DataContracts.JsonHelpers
{
    public class StageJsonConverterHelper<T> : JsonConverter<T> where T : Stage
    {
        private readonly IEnumerable<Type> _types;

        public StageJsonConverterHelper()
        {
            //получаем все типы классов-наследников от Stage
            //они должны быть не абстрактными (иметь конструктор)
            var type = typeof(T);
            _types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
        }

        //переопределенный метод,
        //описывающий возможность этого конвертера принять этот тип
        //чтобы не было зацикливания - проверяем,
        //не является ли базовый тип Stage 
        //!typeToConvert.BaseType.Equals(typeof(Stage))
        public override bool CanConvert(Type typeToConvert) =>
            typeof(Stage).IsAssignableFrom(typeToConvert)
            && !typeToConvert.BaseType.Equals(typeof(Stage));

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                //если не начало объекта
                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException();
                //парсим в документ json
                using (var jsonDocument = JsonDocument.ParseValue(ref reader))
                {
                    //получаем название свойста, которое является дескриптором
                    string decriptorName = nameof(Stage.Type).ToLower();
                    //если нет такого свойства в json кидаем ошибку
                    if (!jsonDocument.RootElement.TryGetProperty(decriptorName, out var typeProperty))
                    {
                        throw new JsonException("Нет поля, обозначающего тип этапа");
                    }

                    //процесс получения типа, к которому мы должны преобразовать
                    //
                    //попробуем преобразовать строку или число к enum
                    // "1" тоже будет преобразовано
                    // в случае успеха получаем строковое представление в ловеркейсе

                    StageType res = StageType.Unknow;
                    string enumName = null;
                    if (typeProperty.ValueKind == JsonValueKind.String 
                        && Enum.TryParse(typeProperty.GetString(), true, out res))
                    {
                        enumName = res.ToString().ToLower();
                    }
                    else if (typeProperty.ValueKind == JsonValueKind.Number
                        && Enum.TryParse(typeProperty.GetByte().ToString(), true, out res))
                    {
                        enumName = res.ToString().ToLower();
                    }
                    else
                    {
                        throw new JsonException("Невозможно преобразовать enum");
                    }

                    //из всех возможных типов находим со схожим названием
                    var type = _types.FirstOrDefault(x => x.Name.ToLower().Contains(enumName));
                    if (type == null)
                        throw new JsonException("Невозможно преобразовать enum");
                    var jsonString = jsonDocument.RootElement.GetRawText();

                    //var jsonObject = (T)JsonSerializer.Deserialize(jsonString, type, options)!;
                    return (T)JsonSerializer.Deserialize(jsonString, type, options)!;
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}
