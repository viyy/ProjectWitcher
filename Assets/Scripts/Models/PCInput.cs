using Assets.Scripts.BaseScripts;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class PCInput
    {
        public enum MouseButtons
        {
            Left = 0,
            Right = 1,
            Center = 2
        }

        public KeyCode Sprint { get; private set; } = KeyCode.LeftShift;

        public KeyCode DefenceButton { get; private set; } = KeyCode.LeftAlt;

        public KeyCode Jump { get; private set; } = KeyCode.Space;

        public KeyCode Roll { get; private set; } = KeyCode.LeftControl;

        public MouseButtons AimMouseButton { get; private set; } = MouseButtons.Right;

        public MouseButtons LeftMouseButton { get; private set; } = MouseButtons.Left;

        public MouseButtons AlternativeFire { get; private set; } = MouseButtons.Center;

        public KeyCode Crouch { get; private set; } = KeyCode.C;

        public KeyCode Inventory { get; private set; } = KeyCode.I;

        public KeyCode ActionButton { get; private set; } = KeyCode.F;

        public KeyCode TargetLock { get; private set; } = KeyCode.T;

        public KeyCode CameraCenter { get; private set; } = KeyCode.H;


    }
}