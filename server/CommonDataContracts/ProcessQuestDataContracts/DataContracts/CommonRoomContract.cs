using CommonInfrastructure.Http;

namespace ProcessQuestDataContracts.DataContracts
{
    public class CommonRoomContract : CommonHttpRequest
    {
        public Guid Key { get; set; }
    }
}
