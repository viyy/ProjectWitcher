using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.BaseScripts;
using Assets.Scripts.Models;


namespace Assets.Scripts.Controllers
{
    public class HealthController : BaseController
    {
        private HealthModel healthModel;

        private float health;
        private float healthMaximum;
       

        public HealthController(ref float health, HealthModel healthModel)
        {
            this.healthModel = healthModel;
            this.health = health;
            healthMaximum = healthModel.healthMaximum;
        }


        public override void ControllerUpdate()
        {
            health = healthModel.health;
            HealthRegeneration(healthModel.healthRegenerationRate);

            health = Mathf.Clamp(health, 0, healthMaximum);

            healthModel.health = health;
        }

        private void HealthRegeneration(float reg)
        {
            health += reg * Time.deltaTime;
        }

    }

}