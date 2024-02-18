using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ProcessQuestService.Core.BusinessLogic;
using ProcessQuestService.Core.InteractionWebSocketModel;

namespace ProcessQuestService.Core.Hubs
{
    [Authorize]
    public class ProcessHub : Hub
    {
        private ConnectQuestLogic _connectQuestLogic;
        private ProcessQuestLogic _processQuestLogic;
        public ProcessHub(ConnectQuestLogic connectQuestLogic, ProcessQuestLogic processQuestLogic) { 
            _connectQuestLogic = connectQuestLogic;
            _processQuestLogic = processQuestLogic;
        }
        public override async Task OnConnectedAsync()
        {
            try
            {
                var u = Context.User;
                var o = Context.User.Claims.ToList();
                //var context = Context.GetHttpContext();
                //var roomKey = context.Request.Query["room"].ToString();
                //var token = context.Request.Query["access_token"].ToString();

                //if (token == null)
                //{
                //    throw new UnauthorizedAccessException("Нет токена доступа!");
                //}
                //if (roomKey == null)
                //{
                //    throw new BadRequestException("Нет ключа доступа комнаты!");
                //}
                //var connectContract = new ProcessConnectContract
                //{
                //    Key = Guid.Parse(roomKey),
                //    Token = token
                //};
                //var user = await _connectQuestLogic.ValidateConnectUserToRoomAsync(connectContract);

                //if(user == null)
                //{
                //    throw new UnauthorizeException();
                //}
                ////добавляем в группу
                //await Groups.AddToGroupAsync(Context.ConnectionId, roomKey);

                //Clients.Group(roomKey).SendAsync("Notify", $"{user.UserName} вошел в чат");

                //занесение идентификатора и метки пользователя в редис?
                await base.OnConnectedAsync();

            }
            catch (Exception ex)
            {
                await base.OnDisconnectedAsync(ex);
            }
        }

        public async Task Send(WebSocketRequest request)
        {
            await _processQuestLogic.ProcessAsync("", request);
        }
    }
}
