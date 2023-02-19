using Microsoft.AspNetCore.Mvc;
using ProcessQuestService.Core.BusinessLogic;
using ProcessQuestService.Core.InteractionWebSocketModel;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ProcessQuestService.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private List<WebSocket> clients;
        private ProcessQuestLogic _processQuestLogic;
        public RoomController(ProcessQuestLogic processQuestLogic)
        {
            _processQuestLogic = processQuestLogic;
            clients = new List<WebSocket>();
        }

        [HttpGet("/room/{room}")]
        public async Task Get(string room)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                
                //clients.Add(webSocket);
                await Echo(webSocket, room);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task Echo(WebSocket webSocket, string room)
        {
            
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);


            while (!result.CloseStatus.HasValue)
            {

                var process = await _processQuestLogic.ProcessAsync(buffer, result.Count, room);

                var request = Encoding.UTF8.GetBytes(process);
                await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            }
            clients.Remove(webSocket);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
