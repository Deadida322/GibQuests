using GenerateQuestsService.DataContracts.Enums;
using GenerateQuestsService.DataContracts.Models.Stages;

namespace GenerateQuestsService.DataContracts.Models
{
    public class Question
    {
        public QuestionType Type { get; set; }

        public int Order { get; set; }

        public string Title { get; set; }

        public string[] Answers { get; set; }

        public string[] RightAnswers { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not Question) return false;

            var compareStage = (obj as Question);
            return compareStage.Title == Title
                && compareStage.Type == Type
                && compareStage.Order == Order;
        }
    }
}
