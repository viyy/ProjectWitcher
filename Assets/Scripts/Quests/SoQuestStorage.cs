using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Quests
{
    public class SoQuestStorage :IQuestStorage
    {
        public Quest GetQuestById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void SaveQuestLog(List<Quest> quests)
        {
            throw new System.NotImplementedException();
        }

        public List<Quest> LoadQuestLog()
        {
            throw new System.NotImplementedException();
        }
    }
}