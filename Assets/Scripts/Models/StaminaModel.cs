using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Models
{
    public class StaminaModel:MonoBehaviour, IDamageable
    {
        public float Stamina = 100;
        public float StaminaMaximum = 100;

        public float StaminaRunCoast = 10f;

        public float StaminaJumpCoast = 20f;

        public float StaminaRollCoast = 30f;

        public float StaminaStandRegenRate = 25f;

        public float StaminaWalkRegenRate = 5f;

        public float StaminaNormalAttackCoast = 10f;

        public float StaminaHeavyAttackCoast = 30f;

        // перенести в модель жизней.
        public void TakeDamage(float damage)
        {
            Debug.Log($"I was hitted for {damage} damage");
        }
    }
}
