using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;

namespace EnemySpace
{
    public class EnemyIdleController
    {
        public delegate void IdleWaiter();
        public static IdleWaiter IdleEvent;

        float idleTime;
        float timer;
        bool animStarted = false;
        Animation anim;

        public void Idle()
        {
            if (!animStarted)
            {
                idleTime = Random.Range(3, 10);
                timer = 0;
                animStarted = true;
                Debug.Log("Wait: " + idleTime);
            }
            else if (animStarted && timer < idleTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                IdleEvent();
                animStarted = false;
            }
        }
    }
}
