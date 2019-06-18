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

        public bool Defence { get; private set; }

        #endregion

        #region Мышь

        public bool Aim { get; private set; }

        public bool LeftClick { get; private set; }

        public bool HeavyAttackClick { get; private set; }

        public float RotationY { get; private set; }

        public float RotationX { get; private set; }

        public float Zoom { get; private set; }

        #endregion

        #region Таймер Тяжелой Атаки

        public float timeToDoubleLeftClick = 1f;

        public float countTimer = 0f;

        public float countLeftClick = 0f;

        public bool isLeftClickUp = false;

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
            #region Проверка на Двойной клик ЛКМ

            if (Input.GetMouseButtonUp((int)PCInputModel.LeftMouseButton))
            {
                isLeftClickUp = true;
                countLeftClick++;                
            }

            if (isLeftClickUp)
            {
                countTimer += Time.deltaTime;
                if(countTimer >= timeToDoubleLeftClick)
                {
                    countTimer = 0;
                    isLeftClickUp = false;
                    countLeftClick = 0;
                    HeavyAttackClick = false;
                }               
            }

            if (countLeftClick >= 2)
            {
                HeavyAttackClick = true;
            }

            #endregion

            ForwardBackward = Input.GetAxis("Horizontal");

            LeftRight = Input.GetAxis("Vertical");

            Jump = Input.GetButton("Jump");

            Run = Input.GetKey(PCInputModel.RunButton);

            RotationY = Input.GetAxis("Mouse X");

            RotationX = -Input.GetAxis("Mouse Y");

            Aim = Input.GetMouseButton((int)PCInputModel.AimMouseButton);            

            LeftClick = Input.GetMouseButtonDown((int)PCInputModel.LeftMouseButton);

            Zoom = Input.GetAxis("Mouse ScrollWheel");

            Roll = Input.GetKeyDown(PCInputModel.RollButton);

            Defence = Input.GetKey(PCInputModel.DefenceButton);
        }
    }
}
