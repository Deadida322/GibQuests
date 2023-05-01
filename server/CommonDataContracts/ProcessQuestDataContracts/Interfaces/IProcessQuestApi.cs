using CommonInfrastructure.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.ViewModels;

namespace ProcessQuestDataContracts.Interfaces
{
    public interface IProcessQuestApi
    {
        [Post("/ProcesssQuest/ConnectToQuest")]
        Task<CommonHttpResponse<StartQuestViewModel>> ConnectToQuestAsync(
        [Body] StartQuestContract contract);
    }
}
