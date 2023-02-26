using CommonInfrastructure.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenerateQuestsService.DataContracts.DataContracts
{
    public class GetQuestContract : CommonHttpRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
