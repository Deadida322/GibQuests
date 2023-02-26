using ProcessQuestDataContracts.Enums;

namespace ProcessQuestDataContracts.Models.Stages
{
    public class TestStageProcess : StageProcess
    {
        public override StageProcessType Type { get; } = StageProcessType.Test;

        public IList<QuestionProcess> Questions { get; set; }
    }
}
