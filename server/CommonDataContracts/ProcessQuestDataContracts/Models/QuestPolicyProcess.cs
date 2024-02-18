using ProcessQuestDataContracts.Enums;

namespace ProcessQuestDataContracts.Models
{
    public class QuestPolicyProcess
    {
        public PolicyProcessType PolicyType { get; set; }

        public MemberProcessType MemberType { get; set; }
    }
}
