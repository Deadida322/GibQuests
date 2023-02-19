using GenerateQuestsService.DataContracts.Enums;

namespace GenerateQuestsService.DataContracts.Models
{
    public class Question
    {
        public QuestionType Type { get; set; }

        public string Title { get; set; }

        public string[] Answers { get; set; }

        public string[] RightAnswers { get; set; }
    }
}
