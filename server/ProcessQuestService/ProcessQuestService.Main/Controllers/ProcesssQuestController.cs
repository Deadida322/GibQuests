using CommonInfrastructure.Http;
using Microsoft.AspNetCore.Mvc;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.Interfaces;
using ProcessQuestDataContracts.ViewModels;
using ProcessQuestService.Core.BusinessLogic;

namespace ProcessQuestService.Main.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProcesssQuestController : ControllerBase, IProcessQuestApi
    {     
        private ConnectQuestLogic _processQuestLogic;
        
        public ProcesssQuestController(ConnectQuestLogic processQuestLogic) {
            _processQuestLogic = processQuestLogic;
        }

        [HttpPost]
        public async Task<CommonHttpResponse<StartQuestViewModel>> RegisterRoomAsync(RegisterRoomContract contract)
        {
            return await _processQuestLogic.RegisterRoomAsync(contract);
        }

        [HttpPost]
        public async Task<CommonHttpResponse<IList<UserProcessingQuestViewModel>>> GetUserQuestsProcessingAsync(CommonHttpRequest contract)
        {
            return await _processQuestLogic.GetUserQuestsProcessingAsync(contract);
        }
    }
}
