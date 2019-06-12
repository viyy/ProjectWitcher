using UnityEngine;
using UnityEditor;

namespace EnemySpace
{
    public class EnemyChase
    {
        public delegate void ChaseContainer(string unityName);
        public static event ChaseContainer ChaseEvent;
        public static event ChaseContainer AttackSwitchEvent;

        float runSpeed;
        EnemyMove move;
        Transform enemyTransform;
        float timer = 0f;
        float chasingTime;
        float priorityDistance;

        public EnemyChase(EnemyMove move, Transform enemyTransform, float runSpeed, float chasingTime, float priorityDistance)
        {
            this.move = move;
            this.enemyTransform = enemyTransform;
            this.runSpeed = runSpeed;
            this.chasingTime = chasingTime;
            this.priorityDistance = priorityDistance;
        }

        /// <summary>
        /// Метод погони
        /// На вход получает центр зоны патрулирования, ее радиус и объект погони
        /// </summary>
        /// <param name="aim"></param>
        public void Chase(GameObject aim, float deltaTime)
        {
            timer += deltaTime;
            ///<summary>
            ///определяем дистанцию от центра зоны
            /// </summary>
            float distance = Mathf.Sqrt(Mathf.Pow(aim.transform.position.x - enemyTransform.position.x, 2) + Mathf.Pow(aim.transform.position.y - enemyTransform.position.y, 2) + Mathf.Pow(aim.transform.position.z - enemyTransform.position.z, 2));
            move.Move(aim.transform.position, runSpeed);
            if (timer > chasingTime)
            {
                StopChase();
                timer = 0f;
            }
            if(distance < priorityDistance)
            {
                AttackSwitchEvent(enemyTransform.name);
            }
        }

        /// <summary>
        /// Метод прекращения погони
        /// </summary>
        public void StopChase()
        {
            ChaseEvent(enemyTransform.name);
        }
    }
}
