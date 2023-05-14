using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Attributes;
using GenerateQuestsService.DataContracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.Interfaces;
using ProcessQuestDataContracts.ViewModels;

namespace QuestCore.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    [AddUserInRequest]
    [ModelStateValidationActionFilter]
    public class ProcessQuestController : ControllerBase
    {
        private IProcessQuestApi _processQuestApi;
        public ProcessQuestController(IProcessQuestApi processQuestApi)
        {
            _processQuestApi = processQuestApi;
        }

        [HttpPost]
        public async Task<CommonHttpResponse<StartQuestViewModel>> ConnectToQuest(StartQuestContract contract)
        {
            return await _processQuestApi.ConnectToQuestAsync(contract);
        }

        [HttpPost]
        public async Task<CommonHttpResponse<IList<UserProcessingQuestViewModel>>> GetUserQuestsProcessing(CommonHttpRequest contract)
        {
            return await _processQuestApi.GetUserQuestsProcessingAsync(contract);
        }
    }
}
