using GenerateQuestsService.DataContracts.Enums;

namespace GenerateQuestsService.DataContracts.Models.Stages
{
    public class QrCodeStage : Stage
    {
        public override StageType Type { get; } = StageType.QrCode;

        public string Code { get; set; }
    }
}
