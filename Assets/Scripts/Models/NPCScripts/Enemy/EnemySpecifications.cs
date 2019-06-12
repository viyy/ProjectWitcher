using UnityEngine;
using UnityEditor;

namespace EnemySpace
{
    [CreateAssetMenu]
    public class EnemySpecifications : ScriptableObject
    {
        [SerializeField] string type = string.Empty;
        public string Type { get { return type; } }

        [SerializeField] float hp = 0;
        public float HP { get { return hp; } }

        [SerializeField] float speed = 0;
        public float Speed { get { return speed; } }

        [SerializeField] float runSpeed = 0;
        public float RunSpeed { get { return runSpeed; } }

        [SerializeField] bool rangeAttack = false;
        public bool RangeAttack { get { return rangeAttack; } }

        [SerializeField] float rangeDamage = 0;
        public float RangeDamage { get { return rangeDamage; } }

        [SerializeField] float rangeDistance = 0;
        public float RangeDistance { get { return rangeDistance; } }

        [SerializeField] float rangeAccuracy = 0;
        public float RangeAccuracy { get { return rangeAccuracy; } }

        [SerializeField] float shootSpeed = 0;
        public float ShootSpeed { get { return shootSpeed; } }

        [SerializeField] bool meleeAttack = false;
        public bool MeleeAttack { get { return meleeAttack; } }

        [SerializeField] float meleeDamage = 0;
        public float MeleeDamage { get { return meleeDamage; } }

        [SerializeField] float hitSpeed = 0;
        public float HitSpeed { get { return hitSpeed; } }

        [SerializeField] float meleeDistance = 0;
        public float MeleeDistance { get { return meleeDistance; } }

        [SerializeField] float viewDistance = 0;
        public float ViewDistance { get { return viewDistance; } }

        [SerializeField] float patrolDistance = 0;
        public float PatrolDistance { get { return patrolDistance; } }

        [SerializeField] float chasingTime = 0;
        public float ChasingTime { get { return chasingTime; } }
    }
}
