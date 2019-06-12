using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;

namespace EnemySpace
{
    public class EnemyIdleController
    {
        public delegate void IdleWaiter(string unitName);
        public static IdleWaiter IdleEvent;

        Transform enemyTransform;
        float idleTime;
        float timer;
        bool animStarted = false;
        Animation anim;

        public EnemyIdleController(Transform enemyTransform)
        {
            this.enemyTransform = enemyTransform;
        }

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
                IdleEvent(enemyTransform.name);
                animStarted = false;
            }
        }
    }
}
