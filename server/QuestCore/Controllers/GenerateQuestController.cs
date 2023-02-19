using AuthService.DataContracts.Interfaces;
using AuthService.DataContracts.User;
using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Attributes;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuestCore.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    [AddUserInRequest]
    [ModelStateValidationActionFilter]
    public class GenerateQuestController : ControllerBase
    {
        private IGenerateQuestsApi _generateQuestsApi;
        public GenerateQuestController(IGenerateQuestsApi generateQuestsApi)
        {
            _generateQuestsApi = generateQuestsApi;
        }

        [HttpPost]
        public Task<CommonHttpResponse> CreateQuest(CreateQuestContract contract)
        {
            return _generateQuestsApi.CreateQuestAsync(contract);
        }

        [HttpPut]
        public Task<CommonHttpResponse<bool>> UpdateQuest(UpdateQuestContract contract)
        {
            return _generateQuestsApi.UpdateQuestAsync(contract);
        }

        //[HttpGet("{id}")]
        //public Task<CommonHttpResponse<QuestViewModel>> GetQuest([FromRoute]GetQuestContract contract)
        //{
        //    return _generateQuestsApi.GetQuestAsync(contract);
        //}

        [HttpPost]
        public Task<CommonHttpResponse<IList<ShortQuestViewModel>>> GetFilteredQuests(GetFilteredQuestsContract contract)
        {
            return _generateQuestsApi.GetFilteredQuestsAsync(contract);
        }

        [HttpDelete("{id}")]
        public Task<CommonHttpResponse<bool>> DeleteQuest([FromRoute]DeleteQuestContract contract)
        {
            return _generateQuestsApi.DeleteQuestAsync(contract);
        }

    }
}
