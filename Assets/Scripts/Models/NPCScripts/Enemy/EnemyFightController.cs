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
        MeshRenderer gun;
        MeshRenderer knife;
        Transform gunBarrelEnd;
        Ray shootRay;
        RaycastHit hit;
        LineRenderer shootLine;
        float priorityDistance;
        float alternativeDistance;
        float switchDistance;
        bool switchMode = true;
        float currentAttackDistance;
        float runSpeed;
        float boostSpeed;
        float timer;
        float rangeDamage;
        float rangeAccuracy;
        float shootSpeed;
        float effectsDisplayTime = 0.1f;
        int layerMask = LayerMask.GetMask("Player");

        public EnemyFightController(EnemyMove move, Transform enemyTransform, MeshRenderer gun, MeshRenderer knife, Transform gunBarrelEnd, LineRenderer shootLine, float priorityDistance, float alternativeDistance, float runSpeed, float rangeDamage, float rangeAccuracy, float shootSpeed)
        {
            this.move = move;
            this.enemyTransform = enemyTransform;
            this.shootLine = shootLine;
            this.priorityDistance = priorityDistance;
            this.alternativeDistance = alternativeDistance;
            switchDistance = Mathf.Abs(priorityDistance - alternativeDistance) / 2;
            currentAttackDistance = priorityDistance;
            this.runSpeed = runSpeed;
            boostSpeed = runSpeed * 2;
            this.rangeDamage = rangeDamage;
            this.rangeAccuracy = rangeAccuracy;
            this.shootSpeed = shootSpeed;
            this.gun = gun;
            this.knife = knife;
            this.gunBarrelEnd = gunBarrelEnd;
        }

        public void Fight(GameObject archrival, float deltaTime)
        {
            timer += deltaTime;
            if (timer > effectsDisplayTime)
                DisableEffects();
            float distance = Mathf.Sqrt(Mathf.Pow(archrival.transform.position.x - enemyTransform.position.x, 2) + Mathf.Pow(archrival.transform.position.y - enemyTransform.position.y, 2) + Mathf.Pow(archrival.transform.position.z - enemyTransform.position.z, 2));
            if(distance > currentAttackDistance)
            {
                Debug.Log("MoveToPlayer");
                move.Continue();
                move.Move(archrival.transform.position, runSpeed);
                move.Rotate(Direction(archrival.transform.position));
                if (switchMode)
                {
                    gun.enabled = true;
                    knife.enabled = false;
                    Debug.Log("MoveToPlayer");
                    move.Continue();
                    move.Move(archrival.transform.position, runSpeed);
                    move.Rotate(Direction(archrival.transform.position));
                }
                else
                {
                    gun.enabled = false;
                    knife.enabled = true;
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
                    gun.enabled = true;
                    knife.enabled = false;
                    move.Rotate(Direction(archrival.transform.position));
                    if(timer >= shootSpeed)
                        RangeAttack(ShootDirection(archrival.transform.position, rangeAccuracy));
                    if (distance <= switchDistance)
                    {
                        switchMode = !switchMode;
                        currentAttackDistance = alternativeDistance;
                    }
                }
                else
                {
                    gun.enabled = false;
                    knife.enabled = true;
                    move.Rotate(Direction(archrival.transform.position));
                    MeleeAttack();
                }
            }
        }

        private Vector3 Direction(Vector3 archrival)
        {
            Vector3 currentDirection = new Vector3(archrival.x - enemyTransform.position.x, 0, archrival.z - enemyTransform.position.z);
            return currentDirection;
        }
        private Vector3 ShootDirection(Vector3 archrival, float rangeAccuracy)
        {
            float chance = Random.Range(-rangeAccuracy, rangeAccuracy);
            Vector3 shootDirection = new Vector3(archrival.x + chance, archrival.y + chance, archrival.z + chance);
            return shootDirection;
        }

        private void RangeAttack(Vector3 archrival)
        {
            timer = 0f;

            float currentDamage = rangeDamage;
            shootLine.enabled = true;
            shootLine.SetPosition(0, gunBarrelEnd.position);
            shootLine.SetPosition(1, archrival);

            shootRay.origin = gunBarrelEnd.position;
            shootRay.direction = archrival;

            if(Physics.Raycast(shootRay, out hit, priorityDistance * 2, layerMask))
            {
                Debug.Log("Hit");
                SetDamage(hit.transform.GetComponent<ISetDamage>(), currentDamage);
            }
        }
        private void MeleeAttack()
        {

        }

        private void DisableEffects()
        {
            shootLine.enabled = false;
        }

        private void SetDamage(ISetDamage obj, float damage)
        {
            if(obj != null)
            {
                obj.ApplyDamage(damage);
            }
        }
    }
}
