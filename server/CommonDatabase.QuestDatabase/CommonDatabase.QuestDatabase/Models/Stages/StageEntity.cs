using System.ComponentModel.DataAnnotations.Schema;

namespace CommonDatabase.QuestDatabase.Models.Stages
{
    public abstract class StageEntity : BaseEntity
    {
        public string Title { get; set; }

        public int Order { get; set; }

        public byte StageType { get; set; }

        public QuestEntity Quest { get; set; }
    }
}
