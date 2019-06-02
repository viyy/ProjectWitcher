using UnityEngine;

namespace Assets.Scripts.Models
{
    public class StaminaModel:MonoBehaviour
    {
        public float Stamina = 100;
        public float StaminaMaximum = 100;

        public float StaminaRunCoast = 10f;

        public float StaminaJumpCoast = 20f;

        public float StaminaRollCoast = 30f;

        public float StaminaStandRegenRate = 25f;

        public float StaminaWalkRegenRate = 5f;
    }
}
