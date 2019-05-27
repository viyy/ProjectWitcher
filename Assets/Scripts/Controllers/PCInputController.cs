using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.BaseScripts;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    class PCInputController : BaseController
    {
        #region Клавиатура

        public float ForwardBackward { get; private set; }

        public float LeftRight { get; private set; }

        public bool Jump { get; private set; }

        public bool Run { get; private set; }

        #endregion

        #region Мышь

        public bool Aim { get; private set; }

        public float RotationY { get; private set; }

        public float RotationX { get; private set; }

        public float Zoom { get; private set; }

        #endregion

        public PCInput PCInputModel;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PCInputModel">Модель ввода игрока</param>
        public PCInputController(PCInput PCInputModel)
        {
            this.PCInputModel = PCInputModel;
        }
        
        public override void ControllerUpdate()
        {
            ForwardBackward = Input.GetAxis("Horizontal");

            LeftRight = Input.GetAxis("Vertical");

            Jump = Input.GetKeyDown(PCInputModel.JumpButton);

            Run = Input.GetKey(PCInputModel.RunButton);

            RotationY = Input.GetAxis("Mouse X");

            RotationX = -Input.GetAxis("Mouse Y");

            Aim = Input.GetMouseButton((int)PCInputModel.AimMouseButton);

            Zoom = Input.GetAxis("Mouse ScrollWheel");
        }
    }
}
