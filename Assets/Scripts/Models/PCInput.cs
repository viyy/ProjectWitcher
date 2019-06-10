using Assets.Scripts.BaseScripts;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class PCInput
    {
        public enum MouseButtons
        {
            Left = 0,
            Right = 1
        }

        public KeyCode RunButton { get; private set; } = KeyCode.LeftShift;

        public KeyCode DefenceButton { get; private set; } = KeyCode.LeftAlt;

        public KeyCode JumpButton { get; private set; } = KeyCode.Space;

        public KeyCode RollButton { get; private set; } = KeyCode.LeftControl;

        public MouseButtons AimMouseButton { get; private set; } = MouseButtons.Right;

        public MouseButtons LeftMouseButton { get; private set; } = MouseButtons.Left;
    }
}