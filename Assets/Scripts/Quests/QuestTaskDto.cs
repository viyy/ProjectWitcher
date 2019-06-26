using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "-1", menuName = "Quest/QuestTask")]
    public class QuestTaskDto : ScriptableObject
    {
        [SerializeField] private string _description;

        [SerializeField] private int _neededAmount;

        [SerializeField] private int _targetId;
        [SerializeField] private QuestTaskTypes _type;

        public QuestTaskTypes Type
        {
            get => _type;
            set => _type = value;
        }

        public int TargetId
        {
            get => _targetId;
            set => _targetId = value;
        }

        public int NeededAmount
        {
            get => _neededAmount;
            set => _neededAmount = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }
    }
}