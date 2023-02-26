using GenerateQuestsService.DataContracts.Enums;

namespace GenerateQuestsService.DataContracts.Models.Stages
{
    public class TestStage : Stage
    {
        public override StageType Type { get; } = StageType.Test;

        public IList<Question> Questions { get; set; }
    }
}
