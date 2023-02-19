using ProcessQuestService.Core.InteractionWebSocketModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessQuestService.Core.Helpers
{
    public static class BuildWebSocketResponseHelper
    {
        public static WebSocketResponse BuildErrorModelResponse()
        {
            return new WebSocketResponse {
                Sucsess = false,
                Error = "Ошибка модели запроса"
            };
        }

        public static WebSocketResponse BuildErrorResponse(string error)
        {
            return new WebSocketResponse
            {
                Sucsess = false,
                Error = error
            };
        }

        public static WebSocketResponse BuildErrorChoiseResponse(string error = "Неверный ответ")
        {
            return new WebSocketResponse
            {
                Sucsess = true,
                Error = error
            };
        }
    }
}
