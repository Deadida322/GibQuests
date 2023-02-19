using GenerateQuestsService.DataContracts.Enums;

namespace GenerateQuestsService.DataContracts.Models.Stages
{
    public class MapStage : Stage
    {
        public override StageType Type { get; } = StageType.Map;

        public Coordinates Coords { get; set; }
    }
}
