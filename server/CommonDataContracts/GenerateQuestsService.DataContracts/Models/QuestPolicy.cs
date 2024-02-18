using GenerateQuestsService.DataContracts.Enums;

namespace GenerateQuestsService.DataContracts.Models
{
    public class QuestPolicy
    {
        public PolicyType PolicyType { get; set; }

        public MemberType MemberType { get; set; }
    }
}
