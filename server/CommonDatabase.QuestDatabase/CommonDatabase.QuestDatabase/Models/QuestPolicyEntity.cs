using System.ComponentModel.DataAnnotations.Schema;

namespace CommonDatabase.QuestDatabase.Models
{
    public class QuestPolicyEntity : BaseEntity
    {
        public byte PolicyType { get; set; }

        public byte MemberType { get; set; }

        [ForeignKey("quest_id")]
        public QuestEntity Quest { get; set; }
    }
}
