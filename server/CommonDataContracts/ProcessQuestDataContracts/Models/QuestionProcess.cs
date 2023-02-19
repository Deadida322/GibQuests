using ProcessQuestDataContracts.Enums;

namespace ProcessQuestDataContracts.Models
{
    public class QuestionProcess
    {
        public QuestionProcessType Type { get; set; }

        public string Title { get; set; }

        public string[] Answers { get; set; }
    }
}
