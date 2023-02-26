using CommonInfrastructure.Http;

namespace GenerateQuestsService.DataContracts.DataContracts
{
    public class GetFilteredQuestsContract : CommonHttpRequest
    {

        public int? Page { get; set; }

        public int? UserId { get; set; }

        public int? Count { get; set; }

    }
}
