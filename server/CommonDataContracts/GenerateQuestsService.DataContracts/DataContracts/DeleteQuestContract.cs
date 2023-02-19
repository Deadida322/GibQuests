using CommonInfrastructure.Http;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace GenerateQuestsService.DataContracts.DataContracts
{
    public class DeleteQuestContract : CommonHttpRequest
    {
        [FromRoute(Name = "id")]
        [AliasAs("id")]
        //[JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
