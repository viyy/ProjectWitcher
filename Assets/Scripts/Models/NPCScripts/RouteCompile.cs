using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс-генератор маршрута из пула точек
/// </summary>
public class RouteCompile : MonoBehaviour
{
    int lastNum;
    float X;
    float Z;

    public Vector3[] Compile(Vector3 startPosition, float range)
    {
        int length = Random.Range(4, 10);
        Debug.Log("Length: " + length);
        Vector3[] route = new Vector3[length];
        for(int i = 0; i < length; i++)
        {
            if(i == 0)
            {
                X = Random.Range(startPosition.x - range, startPosition.x + range);
                Z = Mathf.Sqrt(Mathf.Pow(range, 2) - Mathf.Pow(X, 2));

            }
            else if(i%2 == 0)
            {
                X = Random.Range(0, startPosition.x + range);
                Z = Mathf.Sqrt(Mathf.Pow(range, 2) - Mathf.Pow(X, 2));
            }
            else if(i%3 == 0)
            {
                X = Random.Range(startPosition.x - range, 0);
                Z = Mathf.Sqrt(Mathf.Pow(range, 2) - Mathf.Pow(X, 2));
            }
            else
            {
                X = -X;
                Z = -Mathf.Sqrt(Mathf.Pow(range, 2) - Mathf.Pow(X, 2));
            }
            
            route[i] = new Vector3(X, 0, Z);

        }
        Debug.Log("Route created");
        return route;
    }
}
