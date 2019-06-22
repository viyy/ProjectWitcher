using System.Collections.Generic;
using Quests;

namespace Assets.Scripts.Interfaces
{
    public interface IQuestStorage
    {
        Quest GetQuestById(int id);
        void SaveQuestLog(List<Quest> quests);
        List<Quest> LoadQuestLog();
    }
}