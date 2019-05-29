using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс-генератор маршрута из пула точек
/// </summary>
public class RouteCompile : MonoBehaviour
{
    [SerializeField]private Transform pointsPull; //Пул точек (Необходимо добавить через инспектор)
    private Transform[] pull;
    int lastNum;

    private void Awake()
    {
        lastNum = -1;
        pull = new Transform[pointsPull.childCount];
        for(int i = 0; i < pull.Length; i++)
        {
            pull[i] = pointsPull.GetChild(i);
        }
    }

    public Transform[] Compile()
    {
        int length = Random.Range(4, 10);
        Debug.Log("Length: " + length);
        Transform[] route = new Transform[length];
        for(int i = 0; i < length;)
        {
            int randPoint = Random.Range(0, pull.Length);
            if(lastNum != randPoint)
            {
                route[i] = pull[randPoint];
                lastNum = randPoint;
                i++;

            }           
        }
        Debug.Log("Route created");
        return route;
    }
}
