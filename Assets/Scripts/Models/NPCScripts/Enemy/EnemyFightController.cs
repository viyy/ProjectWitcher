using UnityEngine;
using UnityEditor;

namespace EnemySpace
{
    public class EnemyFightController
    {
        EnemyMeleeAttack meleeAttack;
        EnemyRangeAttack rangeAttack;
        EnemyMove move;
        Transform enemyTransform;
        float priorityDistance;
        float alternativeDistance;
        float switchDistance;
        bool switchMode = true;
        float currentAttackDistance;
        float runSpeed;
        float boostSpeed;

        public EnemyFightController(EnemyMeleeAttack meleeAttack, EnemyRangeAttack rangeAttack, EnemyMove move, Transform enemyTransform, float priorityDistance, float alternativeDistance, float runSpeed)
        {
            this.meleeAttack = meleeAttack;
            this.rangeAttack = rangeAttack;
            this.move = move;
            this.enemyTransform = enemyTransform;
            this.priorityDistance = priorityDistance;
            this.alternativeDistance = alternativeDistance;
            switchDistance = Mathf.Abs(priorityDistance - alternativeDistance) / 2;
            currentAttackDistance = priorityDistance;
            this.runSpeed = runSpeed;
            boostSpeed = runSpeed * 2;
        }

        public void Fight(GameObject archrival)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(archrival.transform.position.x - enemyTransform.position.x, 2) + Mathf.Pow(archrival.transform.position.y - enemyTransform.position.y, 2) + Mathf.Pow(archrival.transform.position.z - enemyTransform.position.z, 2));
            if(distance > currentAttackDistance)
            {
                Debug.Log("MoveToPlayer");
                move.Continue();
                move.Move(archrival.transform.position, runSpeed);
                move.Rotate(Direction(archrival.transform.position));
                if (switchMode)
                {
                    Debug.Log("MoveToPlayer");
                    move.Continue();
                    move.Move(archrival.transform.position, runSpeed);
                    move.Rotate(Direction(archrival.transform.position));
                }
                else
                {
                    Debug.Log("ThrowToPlayer");
                    move.Continue();
                    move.Move(archrival.transform.position, boostSpeed);
                    move.Rotate(Direction(archrival.transform.position));
                    if (distance >= switchDistance)
                    {
                        switchMode = !switchMode;
                        currentAttackDistance = priorityDistance;
                    }
                }

            }
            else if(distance <= currentAttackDistance)
            {
                move.Stop();
                if (switchMode)
                {
                    move.Rotate(Direction(archrival.transform.position));
                    rangeAttack.Attack();
                    if(distance <= switchDistance)
                    {
                        switchMode = !switchMode;
                        currentAttackDistance = alternativeDistance;
                    }
                }
                else
                {
                    move.Rotate(Direction(archrival.transform.position));
                    meleeAttack.Attack();
                }
            }
        }

        private Vector3 Direction(Vector3 archrival)
        {
            Vector3 currentDirection = new Vector3(archrival.x - enemyTransform.position.x, 0, archrival.z - enemyTransform.position.z);
            return currentDirection;
        }
    }
}
