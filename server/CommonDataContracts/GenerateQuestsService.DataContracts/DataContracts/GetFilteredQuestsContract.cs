using CommonInfrastructure.Http;

namespace GenerateQuestsService.DataContracts.DataContracts
{
    public class GetFilteredQuestsContract : CommonHttpRequest
    {

        public bool IsFilterByUser { get; set; } = false;

        public int? Page { get; set; }

        public int? Count { get; set; }

    }
}
