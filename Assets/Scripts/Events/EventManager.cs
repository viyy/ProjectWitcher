using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Events
{
    public class EventManager {

        private Dictionary <GameEventTypes, GameEvent> _eventDictionary;

        private static EventManager _eventManager = null;

        private static EventManager Instance
        {
            get
            {
                if (_eventManager!=null) return _eventManager;
                _eventManager = new EventManager();
                _eventManager.Init();
                return _eventManager;
            }
        }

        private void Init ()
        {
            if (_eventDictionary == null)
            {
                _eventDictionary = new Dictionary<GameEventTypes, GameEvent>();
            }
        }

        public static void StartListening (GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (Instance._eventDictionary.TryGetValue (eventName, out var thisEvent))
            {
                thisEvent.AddListener (listener);
            } 
            else
            {
                thisEvent = new GameEvent();
                thisEvent.AddListener (listener);
                Instance._eventDictionary.Add (eventName, thisEvent);
            }
        }

        public static void StopListening (GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (_eventManager == null) return;
            if (Instance._eventDictionary.TryGetValue (eventName, out var thisEvent))
            {
                thisEvent.RemoveListener (listener);
            }
        }

        public static void TriggerEvent (GameEventTypes eventName, EventArgs eventArgs)
        {
            if (Instance._eventDictionary.TryGetValue (eventName, out var thisEvent))
            {
                thisEvent.Invoke(eventArgs);
            }
        }
    }
}
