using ProcessQuestDataContracts.Enums;

namespace ProcessQuestDataContracts.Models.Stages
{
    public class VideoStageProcess : StageProcess
    {
        public override StageProcessType Type { get; } = StageProcessType.Video;

        public string Url { get; set; }

    }
}
