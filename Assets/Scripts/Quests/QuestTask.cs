using Events;
using Events.Args;

namespace Quests
{
    public class QuestTask
    {
        public QuestTaskTypes Type { get; private set; }
        
        public int TargetId { get; private set; }
        
        public int NeededAmount { get; private set; }

        public int CurrentAmount { get; private set; } = 0;

        public bool IsCompleted => CurrentAmount >= NeededAmount;

        public void AddAmount(int amount)
        {
            CurrentAmount += amount;
            EventManager.TriggerEvent(GameEventTypes.QuestTaskUpdated, new TaskUpdatedArgs(Description,CurrentAmount,NeededAmount));
        }

        public string Description {get; private set;}

        public QuestTask(QuestTaskDto dto)
        {
            Type = dto.Type;
            TargetId = dto.TargetId;
            NeededAmount = dto.NeededAmount;
            Description = dto.Description;
        }
    }
}