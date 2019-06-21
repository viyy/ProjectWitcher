using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "-1", menuName = "Quest/QuestMarker")]
    public class QuestMarkerDto : ScriptableObject
    {
        [SerializeField] private int _mapId;

        public int MapId
        {
            get => _mapId;
            set => _mapId = value;
        }

        [SerializeField] private float _x;

        public float X
        {
            get => _x;
            set => _x = value;
        }

        [SerializeField] private float _y;

        public float Y
        {
            get => _y;
            set => _y = value;
        }
    }
}