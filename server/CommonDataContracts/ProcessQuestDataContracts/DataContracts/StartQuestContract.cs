using CommonInfrastructure.Http;

namespace ProcessQuestDataContracts.DataContracts
{
    public class StartQuestContract : CommonHttpRequest
    {
        public int Id { get; set; }

    }
}
