using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "-1", menuName = "Quest/QuestTask")]
    public class QuestTaskDto : ScriptableObject
    {
        [SerializeField] private QuestTaskTypes _type;

        public QuestTaskTypes Type
        {
            get => _type;
            set => _type = value;
        }

        [SerializeField] private int _targetId;

        public int TargetId
        {
            get => _targetId;
            set => _targetId = value;
        }

        [SerializeField] private int _neededAmount;

        public int NeededAmount
        {
            get => _neededAmount;
            set => _neededAmount = value;
        }

        [SerializeField] private string _description;

        public string Description
        {
            get => _description;
            set => _description = value;
        }
    }
}