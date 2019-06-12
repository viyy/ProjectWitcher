using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.BaseScripts;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controllers
{

    public class EnemyAttackController : MonoBehaviour
    {
        private float damage = 10;
        private Collider target;
        
        private void AttackTarget()
        {
            
            {
                Debug.Log($"target  attack {target}");
                IDamageable d = target.GetComponent<IDamageable>();
                if (d != null)
                {
                    d.TakeDamage(damage);
                }

            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            target = collider;
            Debug.Log($"Detect TARGETDETECTOR => {target}");

            AttackTarget();

        }







    }




}