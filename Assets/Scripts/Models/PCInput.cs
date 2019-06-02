using Assets.Scripts.BaseScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class PCInput
    {
        public enum MouseButtons
        {
            Right = 1
        }

        public KeyCode RunButton { get; private set; } = KeyCode.LeftShift;

        public MouseButtons AimMouseButton { get; private set; } = MouseButtons.Right;

        public KeyCode JumpButton { get; private set; } = KeyCode.Space;

        public KeyCode RollButton { get; private set; } = KeyCode.LeftControl;
        
    }
}
