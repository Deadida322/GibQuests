using GenerateQuestsService.DataContracts.Enums;

namespace GenerateQuestsService.DataContracts.Models.Stages
{
    public class TextStage : Stage
    {
        public override StageType Type { get; } = StageType.Text;

        public string Text { get; set; }
    }
}
