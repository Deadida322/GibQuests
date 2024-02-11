using Microsoft.AspNetCore.SignalR;
using ProcessQuestService.Core.InteractionWebSocketModel;

namespace ProcessQuestService.Main.Hubs
{
    public class ProcessHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            //занесение индефикатора и метки пользователя в редис
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }

        public async Task Send(string room, WebSocketRequest request)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            var t = Clients.Group(room);
            //await Clients.All.SendAsync("Receive", message);
        }
    }
}
