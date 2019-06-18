using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Models;

namespace Assets.Scripts.BaseScripts
{
    class StartScript: MonoBehaviour
    {
        #region Список контроллеров

        public CameraController cameraController { get; private set; }

        public InputController inputController { get; private set; }

        public MovementController movementController { get; private set; }

        public StaminaController staminaController { get; private set; }

        public AnimController animController { get; private set; }

        //public EnemyAttackController enemyAttackController { get; private set; }
        
        public HealthController healthController { get; private set; }

        private List<BaseController> AllControllers = new List<BaseController>(6);

        #endregion

        public static StartScript GetStartScript { get; private set; }
        
        private void Awake()
        {
            GetStartScript = this;

            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            Transform CameraCenter = GameObject.FindGameObjectWithTag("CameraCenter").transform;
            GameObject PlayerAnimator = GameObject.FindGameObjectWithTag("PlayerAnimator");

            //Создаем контроллеры
            inputController = new InputController();
            cameraController = new CameraController(Camera.main.GetComponent<CameraModel>(), CameraCenter, Camera.main, inputController);
            movementController = new MovementController(Player.transform, Player.GetComponent<CharacterController>());
            staminaController = new StaminaController(ref Player.GetComponent<StaminaModel>().Stamina, Player.GetComponent<StaminaModel>(), inputController, movementController);
            animController = new AnimController(PlayerAnimator);
           // enemyAttackController = new EnemyAttackController(targetDetector);
            healthController = new HealthController(ref Player.GetComponent<HealthModel>().health, Player.GetComponent<HealthModel>());
            


            #region Добавляем контроллеры в коллекцию

            AllControllers.Add(inputController);
            AllControllers.Add(cameraController);
            AllControllers.Add(movementController);
            AllControllers.Add(staminaController);
            AllControllers.Add(animController);
           // AllControllers.Add(enemyAttackController);
            AllControllers.Add(healthController);

            #endregion
        }

        private void Update()
        {
            //Запускаем Update каждого контроллера
            foreach (var Controller in AllControllers)
            {
                Controller.ControllerUpdate();
            }
        }

        private void LateUpdate()
        {
            cameraController.ControllerLateUpdate();
            movementController.ControllerLateUpdate();
        }
    }
}
