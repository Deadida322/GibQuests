namespace CommonDatabase.QuestDatabase.Models.Stages
{
    public class TestStageEntity : StageEntity
    {
        public IList<QuestionEntity> Questions { get; set; }
    }
}
