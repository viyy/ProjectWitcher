using Events;
using Events.Args;

namespace Quests
{
    public class QuestTask
    {
        public QuestTask(QuestTaskDto dto)
        {
            Type = dto.Type;
            TargetId = dto.TargetId;
            NeededAmount = dto.NeededAmount;
            Description = dto.Description;
        }

        public QuestTaskTypes Type { get; }

        public int TargetId { get; }

        public int NeededAmount { get; }

        public int CurrentAmount { get; private set; }

        public bool IsCompleted => CurrentAmount >= NeededAmount;

        public string Description { get; }

        public void AddAmount(int amount)
        {
            CurrentAmount += amount;
            EventManager.TriggerEvent(GameEventTypes.QuestTaskUpdated,
                new TaskUpdatedArgs(Description, CurrentAmount, NeededAmount));
        }
    }
}