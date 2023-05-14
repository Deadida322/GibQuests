using Microsoft.AspNetCore.Mvc;
using ProcessQuestService.Core.BusinessLogic;
using ProcessQuestService.Core.Helpers;
using System.Diagnostics;
using System.Net.WebSockets;

namespace ProcessQuestService.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckController : ControllerBase
    {
        private CheckQuestLogic _checkQuestLogic;
        public CheckController(CheckQuestLogic checkQuestLogic)
        {
            _checkQuestLogic = checkQuestLogic;
        }

        [HttpGet("/check/{questId}")]
        public async Task Get(string questId)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                //clients.Add(webSocket);
                await Echo(webSocket, questId);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task Echo(WebSocket webSocket, string questId)
        {

            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);


            while (!result.CloseStatus.HasValue)
            {
                await Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    var response = await _checkQuestLogic.GetQuestProcessingSerializeAsync(questId);
                                                  
                    await webSocket.SendAsync(new ArraySegment<byte>(response, 0, response.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                });
            }
            //clients.Remove(webSocket);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
