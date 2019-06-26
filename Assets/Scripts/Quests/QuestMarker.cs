using UnityEngine;

namespace Quests
{
    public class QuestMarker
    {
        public QuestMarker(int mapId, float x, float y)
        {
            MapId = mapId;
            Position = new Vector2(x, y);
        }

        public QuestMarker(QuestMarkerDto dto)
        {
            MapId = dto.MapId;
            Position = new Vector2(dto.X, dto.Y);
        }

        public int MapId { get; }

        public Vector2 Position { get; }
    }
}