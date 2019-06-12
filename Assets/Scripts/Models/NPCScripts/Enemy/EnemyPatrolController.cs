using UnityEngine;

namespace EnemySpace
{
    /// <summary>
    /// Класс контроллирующий патрулирование нпс
    /// </summary>
    public class EnemyPatrolController
    {
        public delegate void PatrolWaiter(string unitName);
        public static event PatrolWaiter PatrolEvent;

        private Vector3 currentPoint;
        private Vector3 currentDirection;
        int count = 0;

        EnemyMove move;
        Transform enemyTransform;

        public EnemyPatrolController(EnemyMove move, Transform local)
        {
            this.move = move;
            enemyTransform = local;
        }

        /// <summary>
        /// Вызываемый извне метод для патрулирования по заданному маршруту
        /// </summary>
        /// <param name="route"></param>
        public void Patrol(Vector3[] route)
        {
            currentPoint = route[count];
            if (Distance() && count < route.Length - 1)
            {
                Debug.Log("Count: " + count);
                count++;
            }
            else if (Distance() && count == route.Length - 1)
            {
                PatrolEvent(enemyTransform.name);
                count = 0;
            }
            else if (!Distance())
            {
                move.Move(currentPoint);
            }
        }
        public void Stop()
        {
            move.Stop();
        }
        /// <summary>
        /// Проверка расстояния нпс до точки перемещения
        /// </summary>
        /// <returns></returns>
        private bool Distance()
        {
            float dist = Mathf.Sqrt(Mathf.Pow(currentPoint.x - enemyTransform.position.x, 2) + Mathf.Pow(currentPoint.y - enemyTransform.position.y, 2) + Mathf.Pow(currentPoint.z - enemyTransform.position.z, 2));
            if (dist > 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
