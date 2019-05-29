using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class NPCController : MonoBehaviour
{
    bool onPatrol; //Находится ли нпс в патруле
    bool interact; // Взаимодействует ли с ним игрок
    bool onIdle; // Бездействует ли нпс
    Transform[] route; //

    private void Awake()
    {
        onPatrol = false;
        onIdle = false;
        interact = false;
        Mediator.InteractEvent += InteractPlayer;
        NPCPatrolController.PatrolEvent += PatrolWaiter;
        NPCIdleController.IdleEvent += IdleWaiter;
    }

    private void FixedUpdate()
    {
        if (interact)
        {

        }
        else if(onPatrol)
        {
            GetComponent<NPCPatrolController>().Patrol(route);
        }
        else if(onIdle)
        {
            
        }
        else
        {
            int choseAct = Random.Range(0, 2);
            if(choseAct == 0)
            {
                onPatrol = true;
                route = GetComponent<RouteCompile>().Compile();
            }
            else
            {
                onIdle = true;
                GetComponent<NPCIdleController>().Idle();
            }
        }
    }

    /// <summary>
    /// Методы подписывающиеся на события для отслеживания состояний
    /// </summary>
    /// <param name="condition"></param>
    #region Subscribers
    private void InteractPlayer(bool condition)
    {
        interact = condition;
    }
    private void PatrolWaiter()
    {
        onPatrol = false;
    }
    private void IdleWaiter()
    {
        onIdle = false;
    }
    #endregion

}
