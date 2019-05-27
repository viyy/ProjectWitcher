using Assets.Scripts.BaseScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class PCInput:BaseObject
    {
        public enum MouseButtons
        {
            Right = 1
        }

        [SerializeField] public KeyCode RunButton = KeyCode.LeftShift;

        [SerializeField] public MouseButtons AimMouseButton = MouseButtons.Right;

        [SerializeField] public KeyCode JumpButton = KeyCode.Space;
    }
}
