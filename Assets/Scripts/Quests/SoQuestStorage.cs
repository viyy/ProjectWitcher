using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;

namespace Quests
{
    public class SoQuestStorage : IQuestStorage
    {
        public Quest GetQuestById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveQuestLog(List<Quest> quests)
        {
            throw new NotImplementedException();
        }

        public List<Quest> LoadQuestLog()
        {
            throw new NotImplementedException();
        }
    }
}