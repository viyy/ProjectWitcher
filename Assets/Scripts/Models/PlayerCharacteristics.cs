using UnityEngine;

namespace Assets.Scripts.Models
{
    public class PlayerCharacteristics : MonoBehaviour
    {
        public float Stamina = 100;
        public float StaminaMaximum = 100;
        public float RunStaminaDrain = 0.5f;
        public float StaminaJumpCoast = 30f;
        public float StaminaRegenRate = 0.1f;
        public float StaminaRollCoast = 5f;

        private void Update()
        {
            Debug.Log("PC Stamina: "+Stamina);
        }
    }
}
