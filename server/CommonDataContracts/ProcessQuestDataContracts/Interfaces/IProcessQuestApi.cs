using CommonInfrastructure.Http;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.ViewModels;
using Refit;

namespace ProcessQuestDataContracts.Interfaces
{
    public interface IProcessQuestApi
    {
        //[Post("/ProcesssQuest/GetUserQuestsProcessing")]
        //Task<CommonHttpResponse<IList<UserProcessingQuestViewModel>>> GetUserQuestsProcessingAsync(
        //    [Body] CommonHttpRequest contract);


        [Post("/ProcesssQuest/RegisterRoom")]
        Task<CommonHttpResponse<RegisterRoomViewModel>> RegisterRoomAsync([Body] RegisterRoomContract contract);

        [Post("/ProcesssQuest/ConnectToRoom")]
        Task<CommonHttpResponse<ConnectToRoomViewModel>> ConnectToRoomAsync([Body] CommonRoomContract contract);

        [Post("/ProcesssQuest/StartRoom")]
        Task<CommonHttpResponse> StartRoomAsync([Body] CommonRoomContract contract);

    }
}
