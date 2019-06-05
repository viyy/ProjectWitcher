using UnityEngine;
using UnityEditor;

public class EnemyChase : MonoBehaviour
{
    public delegate void ChaseContainer();
    public static event ChaseContainer ChaseEvent;

    float speed = 8;

    /// <summary>
    /// Метод погони
    /// На вход получает центр зоны патрулирования, ее радиус и объект погони
    /// </summary>
    /// <param name="center"></param>
    /// <param name="range"></param>
    /// <param name="aim"></param>
    public void Chase(Vector3 center, float range, GameObject aim)
    {
        ///<summary>
        ///определяем дистанцию от центра зоны
        /// </summary>
        float distance = Mathf.Sqrt(Mathf.Pow(center.x - transform.position.x, 2) + Mathf.Pow(center.y - transform.position.y, 2) + Mathf.Pow(center.z - transform.position.z, 2));
        GetComponent<NPCMove>().Move(aim.transform.position, speed);
        if(distance > range * 2f)
        {
            StopChase();
            Debug.Log("StopChase: \n dist: " + distance);
            Debug.Log("range: " + range);
            Debug.Log("startPosition: " + center);
        }
    }

    /// <summary>
    /// Метод прекращения погони
    /// </summary>
    public void StopChase()
    {
        GetComponent<NPCMove>().Move(transform.position);
        ChaseEvent();

    }
}