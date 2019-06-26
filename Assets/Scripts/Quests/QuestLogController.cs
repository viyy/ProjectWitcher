using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.BaseScripts;
using Assets.Scripts.Interfaces;
using Events;
using Events.Args;

namespace Quests
{
    public class QuestLogController : BaseController
    {
        private readonly List<Quest> _quests;
        private readonly IQuestStorage _questStorage;

        public QuestLogController(IQuestStorage questStorage)
        {
            _questStorage = questStorage;
            _quests = _questStorage.LoadQuestLog();
            EventManager.StartListening(GameEventTypes.QuestAccepted, OnQuestAccept);
            EventManager.StartListening(GameEventTypes.NpcDie, OnNpcDie);
            EventManager.StartListening(GameEventTypes.AreaEnter, OnAreaEnter);
            EventManager.StartListening(GameEventTypes.Saving, OnProgressSaving);
        }

        private void OnProgressSaving(EventArgs arg0)
        {
            _questStorage.SaveQuestLog(_quests);
        }

        private void OnQuestAccept(EventArgs args)
        {
            if (!(args is IdArgs idArgs)) return;
            var t = _questStorage.GetQuestById(idArgs.Id);
            if (t != null)
                _quests.Add(t);
        }

        public List<Quest> GetByZone(int zoneId)
        {
            return _quests.FindAll(x => x.ZoneId == zoneId);
        }

        public List<Quest> GetByTaskType(QuestTaskTypes type)
        {
            return _quests.FindAll(x => x.Tasks.Any(y => y.Type == type));
        }

        public List<Quest> GetTracked()
        {
            return _quests.FindAll(x => x.IsTracked);
        }

        public void QuestUpdate(QuestTaskTypes eventType, int targetId, int amount = 1)
        {
            foreach (var quest in GetByTaskType(eventType))
            foreach (var task in quest.Tasks)
            {
                if (task.Type != eventType || task.TargetId != targetId) continue;
                task.AddAmount(amount);
            }
        }

        private void OnNpcDie(EventArgs args)
        {
            if (!(args is IdArgs dieArgs)) return;
            QuestUpdate(QuestTaskTypes.KillNpc, dieArgs.Id);
        }

        private void OnAreaEnter(EventArgs args)
        {
            if (!(args is IdArgs idArgs)) return;
            QuestUpdate(QuestTaskTypes.FindLocation, idArgs.Id);
        }

        public override void ControllerUpdate()
        {
        }
    }
}