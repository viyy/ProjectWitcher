using UnityEngine;
using UnityEditor;

namespace EnemySpace
{
    public class EnemyComingHome
    {
        public delegate void ComingHomeContainer(string unitName);
        public static event ComingHomeContainer ComingHomeEvent;

        EnemyMove move;
        Vector3 homePoint;
        Transform enemyTransform;

        public EnemyComingHome(EnemyMove move, Transform enemyTransform, Vector3 homePoint)
        {
            this.move = move;
            this.enemyTransform = enemyTransform;
            this.homePoint = homePoint;
        }

        public void ComingHome()
        {
            float distance = Mathf.Sqrt(Mathf.Pow(homePoint.x - enemyTransform.position.x, 2) + Mathf.Pow(homePoint.y - enemyTransform.position.y, 2) + Mathf.Pow(homePoint.z - enemyTransform.position.z, 2));
            move.Move(homePoint);
            if(distance < 2f)
            {
                ComingHomeEvent(enemyTransform.name);
            }
        }
    }
}
