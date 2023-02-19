using ProcessQuestDataContracts.Enums;

namespace ProcessQuestDataContracts.Models.Stages
{
    public class StageProcess
    {
        public int Order { get; set; }

        public string Title { get; set; }

        public virtual StageProcessType Type { get; }
    }
}
