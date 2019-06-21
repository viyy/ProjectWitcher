using System;
using System.Runtime.Serialization;

namespace Quests
{
    [Serializable]
    public enum QuestTaskTypes
    {
        [EnumMember] None = 0,
        [EnumMember] KillNpc = 1,
        [EnumMember] CollectItem = 2,
        [EnumMember] TalkWithNpc = 3,
        [EnumMember] UseObject = 4,
        [EnumMember] FindLocation = 5
    }
}