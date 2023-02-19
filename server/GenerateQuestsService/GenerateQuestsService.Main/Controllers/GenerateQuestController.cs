using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Attributes;
using GenerateQuestsService.Core.BusinessLogic;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace GenerateQuestsService.Main.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GenerateQuestController : Controller, IGenerateQuestsApi
    {
        private readonly GenerateQuestLogic _generateQuestLogic;
        public GenerateQuestController(GenerateQuestLogic generateQuestLogic)
        {
            _generateQuestLogic = generateQuestLogic;
        }

        [HttpPost]
        public Task<CommonHttpResponse> CreateQuestAsync(CreateQuestContract quest)
        {
            return _generateQuestLogic.CreateQuestAsync(quest);
        }

        [HttpPut]
        public Task<CommonHttpResponse<bool>> UpdateQuestAsync(UpdateQuestContract contract)
        {
            return _generateQuestLogic.UpdateQuestAsync(contract);
        }

        [HttpPost]
        public Task<CommonHttpResponse<QuestViewModel>> GetQuestAsync(GetQuestContract contract)
        {
            return _generateQuestLogic.GetQuestAsync(contract);
        }

        [HttpPost]
        public Task<CommonHttpResponse<IList<ShortQuestViewModel>>> GetFilteredQuestsAsync(GetFilteredQuestsContract contract)
        {
            return _generateQuestLogic.GetFilteredQuestsAsync(contract);
        }

        [HttpDelete]
        public Task<CommonHttpResponse<bool>> DeleteQuestAsync(DeleteQuestContract contract)
        {
            return _generateQuestLogic.DeleteQuestAsync(contract);
        }
    }
}
