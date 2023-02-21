using ProcessQuestDataContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessQuestDataContracts.Models.Stages
{
    public class FinalStageProcess : StageProcess
    {
        public override StageProcessType Type { get; } = StageProcessType.Final;
    }
}
