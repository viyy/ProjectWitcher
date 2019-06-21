using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "-1", menuName = "Quest/Quest")]
    public class QuestDto : ScriptableObject
    {
        [SerializeField] private int _id;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        [SerializeField] private List<int> _requiredQuests = new List<int>();

        public List<int> RequiredQuests
        {
            get => _requiredQuests;
            set => _requiredQuests = value;
        }

        [SerializeField] private string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [SerializeField] private string _description;

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        [SerializeField] private int _zoneId;

        public int ZoneId
        {
            get => _zoneId;
            set => _zoneId = value;
        }

        [SerializeField] private List<QuestMarkerDto> _mapMarkers = new List<QuestMarkerDto>();

        public List<QuestMarkerDto> MapMarkers
        {
            get => _mapMarkers;
            set => _mapMarkers = value;
        }

        [SerializeField] private List<QuestTaskDto> _tasks = new List<QuestTaskDto>();

        public List<QuestTaskDto> Tasks
        {
            get => _tasks;
            set => _tasks = value;
        }

        //TODO: public QuestRewardDto Reward { get; set; } = new QuestRewardDto();
    }
}
