using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.BaseScripts;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controllers
{

    public class EnemyAttackController : BaseController
    {
        private float damage = 10;
        private Collider target;
        TargetDetector targetDetector;

        public EnemyAttackController(TargetDetector targetDetector)
        {
            this.targetDetector = targetDetector;
        }




        private void AttackTarget()
        {
            target = targetDetector.target;

            if (target == null)
            {
                return;
            }
            else if (target.tag == "Player")
            {

                Debug.Log($"target  attack {target}");

                IDamageable d = target.GetComponent<IDamageable>();
                if (d != null)
                {
                    d.TakeDamage(damage);
                }
                

            }
        }



        public override void ControllerUpdate()
        {
            AttackTarget();
        }


    }




}