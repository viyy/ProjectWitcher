using System.Collections.Generic;

namespace Quests
{
    public class Quest
    {
        public int Id { get; }
        
        public List<int> RequiredQuests { get; }
        public string Name { get; }
        
        public string Description { get; }

        public List<QuestTask> Tasks { get; } = new List<QuestTask>();

        //TODO: public QuestReward Reward { get; }

        public bool IsTracked { get; set; } = false;
        public int ZoneId { get; }
        
        public List<QuestMarker> MapMarkers { get; }

        public Quest(QuestDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            ZoneId = dto.ZoneId;
            MapMarkers = new List<QuestMarker>();
            foreach (var mapMarker in dto.MapMarkers)
            {
                MapMarkers.Add(new QuestMarker(mapMarker.MapId, mapMarker.X, mapMarker.Y));
            }
            RequiredQuests = dto.RequiredQuests;
            foreach (var task in dto.Tasks)
            {
                Tasks.Add(new QuestTask(task));
            }
            //TODO: Reward = new QuestReward(dto.Reward);
        }
    }
}