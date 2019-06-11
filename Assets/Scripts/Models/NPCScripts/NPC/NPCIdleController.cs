using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCIdleController : MonoBehaviour
{
    public delegate void IdleWaiter();
    public static IdleWaiter IdleEvent;

    Coroutine waiter;
    float time;

    IEnumerator WaitForEnd(float time)
    {
        WaitForSeconds wait = new WaitForSeconds(time);
        yield return wait;
        IdleEvent();
    }

    public void Idle()
    {
        time = Random.Range(3, 10);
        Debug.Log("Wait: " + time);
        waiter = StartCoroutine(WaitForEnd(time));
    }
}