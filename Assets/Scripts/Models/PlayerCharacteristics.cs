using UnityEngine;
using Assets.Scripts.BaseScripts;

namespace Assets.Scripts.Models
{
    public class PlayerCharacteristics:BaseObject
    {
        [Header("Player Stamina Settings")]
        [SerializeField] public float Stamina = 100;
        [SerializeField] public float StaminaMaximum = 100;
        [SerializeField] public float RunStaminaDrain = 0.5f;
        [SerializeField] public float StaminaJumpCoast = 30f;
        [SerializeField] public float StaminaRegenRate = 0.1f;
    }
}
