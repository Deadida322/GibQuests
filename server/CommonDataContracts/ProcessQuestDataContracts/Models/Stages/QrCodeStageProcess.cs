using ProcessQuestDataContracts.Enums;

namespace ProcessQuestDataContracts.Models.Stages
{
    public class QrCodeStageProcess : StageProcess
    {
        public override StageProcessType Type { get; } = StageProcessType.QrCode;

        public string Code { get; set; }
    }
}
