using GenerateQuestsService.DataContracts.Enums;

namespace GenerateQuestsService.DataContracts.Models.Stages
{
    public class VideoStage : Stage
    {
        public override StageType Type { get;} = StageType.Video;

        public string Url { get; set; }

    }
}
