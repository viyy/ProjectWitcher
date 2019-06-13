using UnityEngine;
using UnityEditor;
using Assets.Scripts.Interfaces;

namespace EnemySpace
{
    public class EnemyFightController
    {
        public delegate void AttackToChase(string unitName);
        public static event AttackToChase AttackToChaseEvent;

        EnemyMove move;
        Transform enemyTransform;
        MeshRenderer gun;
        MeshRenderer knife;
        Transform gunBarrelEnd;
        AudioSource gunShotSound;
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
        float meleeDamage;
        float hitSpeed;
        float effectsDisplayTime = 0.1f;
        int layerMask = LayerMask.GetMask("Player");
        int meleeHitCount = 0;
        bool specialAbility = false;
        float hitChance;
        float missChance;

        int shotCount = 1;
        int hitCount = 1;

        public EnemyFightController(EnemyMove move, Transform enemyTransform, MeshRenderer gun, MeshRenderer knife, Transform gunBarrelEnd, LineRenderer shootLine, float priorityDistance, float alternativeDistance, float runSpeed, float rangeDamage, float rangeAccuracy, float shootSpeed, float meleeDamage, float hitSpeed, AudioSource gunShotSound)
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
            this.gunShotSound = gunShotSound;
            this.meleeDamage = meleeDamage;
            this.hitSpeed = hitSpeed;
            hitChance = rangeAccuracy;
            missChance = 10 - rangeAccuracy;
        }

        public void Fight(GameObject archrival, float deltaTime)
        {
            timer += deltaTime;
            SpecialAbilityActivator();
            if (timer > effectsDisplayTime)
                DisableEffects();
            if (specialAbility)
            {
                SpecialAbility(archrival.transform.position);
            }
            else
            {
                float distance = Mathf.Sqrt(Mathf.Pow(archrival.transform.position.x - enemyTransform.position.x, 2) + Mathf.Pow(archrival.transform.position.y - enemyTransform.position.y, 2) + Mathf.Pow(archrival.transform.position.z - enemyTransform.position.z, 2));
                if (distance > currentAttackDistance)
                {
                    Debug.Log("MoveToPlayer");
                    move.Continue();
                    move.Move(archrival.transform.position, runSpeed);
                    move.Rotate(RotateDirection(archrival.transform.position));
                    if (timer > 15f)
                    {
                        AttackToChaseEvent(enemyTransform.name);
                    }
                    if (switchMode)
                    {
                        gun.enabled = true;
                        knife.enabled = false;
                        Debug.Log("MoveToPlayer");
                        move.Continue();
                        move.Move(archrival.transform.position, runSpeed);
                        move.Rotate(RotateDirection(archrival.transform.position));
                    }
                    else
                    {
                        gun.enabled = false;
                        knife.enabled = true;
                        Debug.Log("ThrowToPlayer");
                        move.Continue();
                        move.Move(archrival.transform.position, boostSpeed);
                        move.Rotate(RotateDirection(archrival.transform.position));
                        if (distance >= switchDistance)
                        {
                            timer = 0f;
                            switchMode = !switchMode;
                            currentAttackDistance = priorityDistance;
                        }
                    }

                }
                else if (distance <= currentAttackDistance)
                {
                    move.Stop();
                    if (switchMode)
                    {
                        gun.enabled = true;
                        knife.enabled = false;
                        move.Rotate(RotateDirection(archrival.transform.position));
                        if (timer >= shootSpeed)
                            RangeAttack(ShootDirection(archrival.transform.position, rangeAccuracy));
                        if (distance <= switchDistance)
                        {
                            timer = 0f;
                            switchMode = !switchMode;
                            currentAttackDistance = alternativeDistance;
                        }
                    }
                    else
                    {
                        gun.enabled = false;
                        knife.enabled = true;
                        move.Rotate(RotateDirection(archrival.transform.position));
                        if (timer >= hitSpeed)
                        {
                            MeleeAttack();
                            meleeHitCount++;
                        }
                    }
                }
            }            
        }

        private Vector3 RotateDirection(Vector3 archrival)
        {
            Vector3 currentDirection = new Vector3(archrival.x - enemyTransform.position.x, 0, archrival.z - enemyTransform.position.z);
            return currentDirection;
        }
        private Vector3 ShootDirection(Vector3 archrival, float rangeAccuracy)
        {
            if(hitChance == 0 && missChance == 0)
            {
                hitChance = rangeAccuracy;
                missChance = 10 - rangeAccuracy;
            }
            float isHit = Random.Range(-missChance, hitChance);
            Vector3 currentDirection;
            Vector3 shootDirection = new Vector3(archrival.x - enemyTransform.position.x, archrival.y - enemyTransform.position.y, archrival.z - enemyTransform.position.z);
            if (isHit >= 0 && isHit < hitChance)
            {
                currentDirection = shootDirection;
                Debug.LogWarning("ShotInPlayer");
                hitChance--;
            }
            else
            {
                int miss = Random.Range(-8, -4);
                currentDirection = new Vector3(shootDirection.x + miss, shootDirection.y, shootDirection.z + miss);
                Debug.LogWarning("MissShot");
                missChance--;
            }
            return currentDirection;
        }

        private void RangeAttack(Vector3 archrival)
        {
            Debug.Log("ShotCount: " + shotCount);
            Debug.Log("HitCount: " + hitCount);
            timer = 0f;
            gunShotSound.Play();
            float currentDamage = rangeDamage;
            shootLine.useWorldSpace = true;
            shootLine.enabled = true;
            shootLine.SetPosition(0, gunBarrelEnd.position);
            shootLine.SetPosition(1, archrival + gunBarrelEnd.position);

            shotCount++;
            shootRay.origin = gunBarrelEnd.position;
            shootRay.direction = archrival;
            Debug.Log(shootRay);

            if(Physics.Raycast(shootRay, out hit, 100f))
            {
                hitCount++;
                Debug.LogError("Hit: " + hit.collider.name);
                SetDamage(hit.collider.GetComponent<IDamageable>(), currentDamage);
            }
        }
        private void MeleeAttack()
        {
            timer = 0f;
        }

        private void DisableEffects()
        {
            shootLine.enabled = false;
            shootLine.useWorldSpace = false;
        }

        private void SetDamage(IDamageable obj, float damage)
        {
            if(obj != null)
            {
                Debug.Log("Damage: " + damage);
                obj.TakeDamage(damage);
            }
        }

        private void SpecialAbilityActivator()
        {
            if(meleeHitCount == 3)
            {
                specialAbility = true;
            }
        }

        private void SpecialAbility(Vector3 archrival)
        {
            Debug.Log("Special");
            switchMode = true;
            currentAttackDistance = priorityDistance;
            gun.enabled = true;
            knife.enabled = false;
            Vector3 direction = RotateDirection(archrival);
            Vector3 distancePoint = -enemyTransform.forward * switchDistance;
            move.Continue();
            move.Move(distancePoint, boostSpeed);
            //enemyTransform.position = enemyTransform.Translate(distancePoint);
            float distance = Mathf.Sqrt(Mathf.Pow(archrival.x - enemyTransform.position.x, 2) + Mathf.Pow(archrival.y - enemyTransform.position.y, 2) + Mathf.Pow(archrival.z - enemyTransform.position.z, 2));
            if (distance >= switchDistance)
            {
                specialAbility = false;
                meleeHitCount = 0;
            }
        }
    }
}
