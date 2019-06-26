using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "-1", menuName = "Quest/QuestMarker")]
    public class QuestMarkerDto : ScriptableObject
    {
        [SerializeField] private int _mapId;

        [SerializeField] private float _x;

        [SerializeField] private float _y;

        public int MapId
        {
            get => _mapId;
            set => _mapId = value;
        }

        public float X
        {
            get => _x;
            set => _x = value;
        }

        public float Y
        {
            get => _y;
            set => _y = value;
        }
    }
}