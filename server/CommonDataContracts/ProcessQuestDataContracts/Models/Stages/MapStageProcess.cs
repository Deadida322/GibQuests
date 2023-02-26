using ProcessQuestDataContracts.Enums;

namespace ProcessQuestDataContracts.Models.Stages
{
    public class MapStageProcess : StageProcess
    {
        public override StageProcessType Type { get; } = StageProcessType.Map;

        public CoordinatesProcess Coords { get; set; }
    }
}
