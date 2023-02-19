using CommonInfrastructure.Http;
using GenerateQuestsService.DataContracts.DataContracts;
using Refit;

namespace GenerateQuestsService.DataContracts.Interfaces
{
    public interface IGenerateQuestsApi
    {
        [Post("/GenerateQuest/CreateQuest")]
          Task<CommonHttpResponse> CreateQuestAsync(
          [Body] CreateQuestContract contract);

        [Put("/GenerateQuest/UpdateQuest")]
        Task<CommonHttpResponse<bool>> UpdateQuestAsync(
          [Body] UpdateQuestContract contract);

        [Post("/GenerateQuest/GetQuest")]
        Task<CommonHttpResponse<QuestViewModel>> GetQuestAsync(
          [Body] GetQuestContract contract);

        [Post("/GenerateQuest/GetFilteredQuests")]
        Task<CommonHttpResponse<IList<ShortQuestViewModel>>> GetFilteredQuestsAsync(
          [Body] GetFilteredQuestsContract contract);

        [Delete("/GenerateQuest/DeleteQuest")]
        Task<CommonHttpResponse<bool>> DeleteQuestAsync(
         [Body] DeleteQuestContract contract);

    }
}
