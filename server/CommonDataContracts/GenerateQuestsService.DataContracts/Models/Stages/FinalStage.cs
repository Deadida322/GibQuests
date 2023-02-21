using GenerateQuestsService.DataContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateQuestsService.DataContracts.Models.Stages
{
    public class FinalStage : Stage
    {
        public override StageType Type { get; } = StageType.Map;
    }
}
