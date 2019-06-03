using Assets.Scripts.BaseScripts;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    class StaminaController : BaseController
    {
        private StaminaModel staminaModel;

        private PCInputController InputController;

        private MovementController movementController;

        private  float Stamina;
        private float StaminaMaximum;

        public bool CanRun { get; private set; }
        private bool RunPress;

        private bool JumpPress;
        public bool CanJump { get; private set; }

        private bool RollPress;
        public bool CanRoll { get; private set; }

        private bool NormalAttackPress;
        public bool CanNormalAttack { get; private set; }

        private bool HeavyAttackPress;
        public bool CanHeavyAttack { get; private set; }

        private bool IsStanding;
        private bool IsWalking;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Stamina">Ссылка на текущее значение стамины</param>
        /// <param name="staminaModel">Ссылка на модель характеристик игрока</param>
        /// <param name="InputController">Ссылка на контроллер ввода</param>
        /// <param name="movementController">Ссылка на контроллер перемещения</param>
        public StaminaController(ref float Stamina, StaminaModel staminaModel, 
            PCInputController InputController, MovementController movementController)
        {
            //Получаем ссылки
            this.Stamina =  Stamina;

            this.staminaModel = staminaModel;

            this.movementController = movementController;

            this.InputController = InputController;

            StaminaMaximum = staminaModel.StaminaMaximum;
        }


        private void GetInputsAndFlags()
        {
            RunPress = InputController.Run;
            JumpPress = InputController.Jump;
            RollPress = InputController.Roll;
            NormalAttackPress = InputController.LeftClick;
            // Реализовать тяжелую атаку !!!!!!!!!!!!!!!!!!!!! HeavyAttackPress = InputController. !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            IsStanding = movementController.IsStanding;
            IsWalking = movementController.IsWalking;
        }


        public override void ControllerUpdate()
        {
            GetInputsAndFlags();

            if (IsStanding)
            {
                Regenerate(staminaModel.StaminaStandRegenRate);
            }
            if (IsWalking)
            {
                Regenerate(staminaModel.StaminaWalkRegenRate);
            }

            RunStaminaDrain();
            JumpStaminaDrain();
            RollStaminaDrain();

            //Ограничиваем значения стамины
            Stamina = Mathf.Clamp(Stamina, 0, StaminaMaximum);
            staminaModel.Stamina = Stamina;
        }

        /// <summary>
        /// Метод регенерации стамины
        /// </summary>
        private void Regenerate(float reg)
        {
            Stamina += reg * Time.deltaTime;
        }

        /// <summary>
        /// Метод расхода стамины на прыжок
        /// </summary>
        /// <param name="JumpPress"></param>
        private void JumpStaminaDrain()
        {
            CanJump = (JumpPress & Stamina > staminaModel.StaminaJumpCoast);

            if(CanJump & movementController.IsGrounded)
            {
                Stamina -= staminaModel.StaminaJumpCoast;
            }
        }

        /// <summary>
        /// Метод расхода стамины на бег
        /// </summary>
        /// <param name="RunPress"></param>
        private void RunStaminaDrain()
        {
            CanRun = ((RunPress & movementController.IsGrounded) & Stamina > 0);

            if(CanRun)
            {
                Stamina -= staminaModel.StaminaRunCoast * Time.deltaTime;
            }
        }

        /// <summary>
        /// Метод расхода стамины на кувырок
        /// </summary>
        private void RollStaminaDrain()
        {
            CanRoll = ((RollPress & movementController.IsGrounded) & Stamina > staminaModel.StaminaRollCoast);

            if(CanRoll)
            {
                Stamina -= staminaModel.StaminaRollCoast;
            }
        }

        /// <summary>
        /// Метод расхода стамины на Обычную Атаку
        /// </summary>
        private void NormalAttackStaminaDrain()
        {
            CanNormalAttack = ((NormalAttackPress & movementController.IsGrounded) & Stamina > staminaModel.StaminaNormalAttackCoast);

            if (CanNormalAttack)
            {
                Stamina -= staminaModel.StaminaNormalAttackCoast;
            }
        }

        /// <summary>
        /// Метод расхода стамины на Тяжелую Атаку
        /// </summary>
        private void HeavyAttackStaminaDrain()
        {
            CanHeavyAttack = ((HeavyAttackPress & movementController.IsGrounded) & Stamina > staminaModel.StaminaNormalAttackCoast);

            if (CanHeavyAttack)
            {
                Stamina -= staminaModel.StaminaHeavyAttackCoast;
            }
        }
    }
}
