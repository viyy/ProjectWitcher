using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "-1", menuName = "Quest/Quest")]
    public class QuestDto : ScriptableObject
    {
        [SerializeField] private string _description;
        [SerializeField] private int _id;

        [SerializeField] private List<QuestMarkerDto> _mapMarkers = new List<QuestMarkerDto>();

        [SerializeField] private int _minLevel;

        [SerializeField] private string _name;

        [SerializeField] private QuestMarkerDto _questEnd;

        [SerializeField] private QuestMarkerDto _questStart;

        [SerializeField] private List<int> _requiredQuests = new List<int>();

        [SerializeField] private List<QuestTaskDto> _tasks = new List<QuestTaskDto>();

        [SerializeField] private int _zoneId;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public List<int> RequiredQuests
        {
            get => _requiredQuests;
            set => _requiredQuests = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public int ZoneId
        {
            get => _zoneId;
            set => _zoneId = value;
        }

        public List<QuestMarkerDto> MapMarkers
        {
            get => _mapMarkers;
            set => _mapMarkers = value;
        }

        public List<QuestTaskDto> Tasks
        {
            get => _tasks;
            set => _tasks = value;
        }

        public QuestMarkerDto QuestStart
        {
            get => _questStart;
            set => _questStart = value;
        }

        public QuestMarkerDto QuestEnd
        {
            get => _questEnd;
            set => _questEnd = value;
        }

        public int MinLevel
        {
            get => _minLevel;
            set => _minLevel = value;
        }

        //TODO: public QuestRewardDto Reward { get; set; } = new QuestRewardDto();
    }
}