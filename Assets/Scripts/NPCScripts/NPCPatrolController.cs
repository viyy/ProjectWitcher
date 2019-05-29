using UnityEngine;

/// <summary>
/// Класс контроллирующий патрулирование нпс
/// </summary>
public class NPCPatrolController : MonoBehaviour
{
    public delegate void PatrolWaiter();
    public static event PatrolWaiter PatrolEvent;

    private Vector3 currentPoint;
    private Vector3 currentDirection;
    int count;

    private void Awake()
    {
        count = 0;
    }

    /// <summary>
    /// Вызываемый извне метод для патрулирования по заданному маршруту
    /// </summary>
    /// <param name="route"></param>
    public void Patrol(Transform[] route)
    {
        currentPoint = route[count].position;
        if (Distance() && count < route.Length - 1)
        {
            Debug.Log("Count: " + count);
            count++;
        }
        else if(Distance() && count == route.Length - 1)
        {
            PatrolEvent();
            count = 0;
        }
        else if (!Distance())
        {
            GetComponent<NPCMove>().Move(Direction());
        }
    }

    /// <summary>
    /// Проверка расстояния нпс до точки перемещения
    /// </summary>
    /// <returns></returns>
    private bool Distance()
    {
        float dist = Mathf.Abs(Mathf.Pow(currentPoint.x - transform.position.x, 2) + Mathf.Pow(currentPoint.y - transform.position.y, 2) + Mathf.Pow(currentPoint.z - transform.position.z, 2));
        if(dist > 2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private Vector3 Direction()
    {
        currentDirection = new Vector3(currentPoint.x - transform.position.x, 0, currentPoint.z - transform.position.z);
        return currentDirection;
    }
}