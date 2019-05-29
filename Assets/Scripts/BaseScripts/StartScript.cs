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

        public PCInputController inputController { get; private set; }

        public MovementController movementController { get; private set; }

        public StaminaController staminaController { get; private set; }

        private List<BaseController> AllControllers;

        #endregion

        public static StartScript GetStartScript { get; private set; }
        
        private void Awake()
        {
            GetStartScript = this;

            GameObject Player = GameObject.FindGameObjectWithTag("Player");

            //Создаем контроллеры
            inputController = new PCInputController(Player.GetComponent<PCInput>());
            cameraController = new CameraController(Camera.main.GetComponent<CameraModel>(), Player.transform, Camera.main);
            movementController = new MovementController(Player.GetComponent<PlayerMovement>(), Player.transform, Player.GetComponent<CharacterController>());
            staminaController = new StaminaController(ref Player.GetComponent<PlayerCharacteristics>().Stamina, Player.GetComponent<PlayerCharacteristics>(), inputController, movementController);
            
            AllControllers = new List<BaseController>
            {
                inputController, cameraController, staminaController, movementController
            };
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
        }
    }
}
