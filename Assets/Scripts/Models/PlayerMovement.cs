using Assets.Scripts.BaseScripts;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class PlayerMovement:BaseObject
    {
        [SerializeField] public float Speed = 5f;

        [SerializeField] public float RunSpeed = 10f;

        [SerializeField] public float RotateSpeed = 15f;

        [SerializeField] public float AimRotateSpeed = 20f;

        [SerializeField] public float WalkStaminaDrain = 0.1f;

        [SerializeField] public float RunStaminaDrain = 0.3f;

        [SerializeField] public float Gravity = -9.81f;

        [SerializeField] public float JumpHeight = 10f;

        [SerializeField] public float GroundRayDistance = 1.2f;

        [SerializeField] public float MinimumFall = -1.0f;

        [SerializeField] public float TerminalFall = -15.0f;
    }
}
