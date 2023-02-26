using ProcessQuestDataContracts.Enums;

namespace ProcessQuestDataContracts.Models.Stages
{
    public class TextStageProcess : StageProcess
    {
        public override StageProcessType Type { get; } = StageProcessType.Text;

        public string Text { get; set; }
    }
}
