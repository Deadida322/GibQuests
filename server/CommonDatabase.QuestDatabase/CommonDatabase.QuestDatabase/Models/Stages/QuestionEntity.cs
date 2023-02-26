using GenerateQuestsService.DataContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDatabase.QuestDatabase.Models.Stages
{
    public class QuestionEntity : BaseEntity
    {
        public byte Type { get; set; }

        public string Title { get; set; }

        public string[] Answers { get; set; }

        public string[] RightAnswers { get; set; }

        [ForeignKey("test_stage_id")]
        public TestStageEntity TestStage { get; set; }
    }
}
