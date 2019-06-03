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

        #region Модель

        public PCInput PCInputModel { get; private set; }

        #endregion

        #region Клавиатура

        public float ForwardBackward { get; private set; }

        public float LeftRight { get; private set; }

        public bool Jump { get; private set; }

        public bool Run { get; private set; }

        public bool Roll { get; private set; }

        #endregion

        #region Мышь

        public bool Aim { get; private set; }

        public bool LeftClick { get; private set; }

        public float RotationY { get; private set; }

        public float RotationX { get; private set; }

        public float Zoom { get; private set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PCInputModel">Модель ввода игрока</param>
        public PCInputController()
        {
            PCInputModel = new PCInput();
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

            LeftClick = Input.GetMouseButton((int)PCInputModel.LeftMouseButton);

            Zoom = Input.GetAxis("Mouse ScrollWheel");

            Roll = Input.GetKeyDown(PCInputModel.RollButton);
        }
    }
}
