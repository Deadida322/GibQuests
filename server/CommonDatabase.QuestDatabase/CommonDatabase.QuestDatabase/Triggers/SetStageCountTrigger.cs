using CommonDatabase.QuestDatabase.Models;
using EntityFrameworkCore.Triggered;

namespace CommonDatabase.QuestDatabase.Triggers
{
    public class SetStageCountTrigger : IBeforeSaveTrigger<QuestEntity>
    {
        public Task BeforeSave(ITriggerContext<QuestEntity> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Added || (context.ChangeType == ChangeType.Modified && !context.Entity.IsDeleted))
            {
                context.Entity.StageCount = context.Entity.Stages.Count;
            }
            return Task.CompletedTask;
        }
    }
}
