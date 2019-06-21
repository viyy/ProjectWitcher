using UnityEngine;

namespace Quests
{
    public class QuestMarker
    {
        public int MapId { get; }
        
        public Vector2 Position { get; }

        public QuestMarker(int mapId, float x, float y)
        {
            MapId = mapId;
            Position = new Vector2(x, y);
        }
    }
}