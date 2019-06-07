using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Models;
using Assets.Scripts.BaseScripts;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    using CameraModel = Assets.Scripts.Models.CameraModel;
    using UnityCamera = UnityEngine.Camera;

    class CameraController : BaseController
    {
        #region Модель

        //Модель камеры
        private CameraModel CameraModel;

        #endregion
        
        //Камера Unity
        public UnityCamera Camera { get; private set; }

        //Положение игрока
        private Transform Player;

        //Позиция камеры при прицеливании
        private Transform AimPosition;

        //Угол вращения камеры по оси Y.
        private float RotationY = 0;

        //Угол вращения камеры по оси X.
        private float RotationX = 0;

        //Приближение камеры.
        private float Zoom;

        //Вектор расстояния между игроком и камерой.
        private Vector3 Offset;

        //Расстояние до камеры с препятствием.
        private Vector3 ObstacleCameraPosition;

        //Позиция камеры без препятствий
        private Vector3 StandartCameraPosition;

        //Стартовое расстояние до камеры.
        private Vector3 StartCameraDistance;

        //Флаг для наличия препятствий
        private bool CameraObstacle;

        //Флаг для прицеливания
        private bool IsAiming;

        //Флаг для возвращение камеры в стандартное положение за спину персонажа
        private bool BackToStandartCameraProsition = false;

        //Луч для проверки препятствия перед камерой.
        private RaycastHit Ray;

        //Ccылка на контроллер ввода
        PCInputController inputController;

        //Кватернион для вращения
        Quaternion Rotation;

        //Вращение по оси Y
        public float YRotation
        {
            get
            {
                return RotationY;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CameraModel">Модель камеры</param>
        /// <param name="Player">Позиция игрока</param>
        /// <param name="Camera">Ссылка на MainCamera</param>
        public CameraController(CameraModel CameraModel, Transform Player, UnityCamera Camera, PCInputController inputController)
        {
            AimPosition = GameObject.FindGameObjectWithTag("AimPosition").transform;

            //Получаем модель для камеры.
            this.CameraModel = CameraModel;

            this.Player = Player;
            this.Camera = Camera;

            //Получаем ссылку на контроллер ввода
            this.inputController = inputController;

            //Вычисляем стартовое положение камеры
            StartCameraDistance = Player.transform.position + (-Vector3.forward * (CameraModel.CameraMinDistance + CameraModel.CameraMaxDistance / 2));

            //Задаем начальное расстояние между камерой и игроком
            Offset = Player.transform.position - StartCameraDistance;
        }
        
        /// <summary>
        /// Update контроллера
        /// </summary>
        public override void ControllerLateUpdate()
        {
            GetInputs();

            //Ограничиваем движение камеры по оси X
            RotationX = (IsAiming) ? Mathf.Clamp(RotationX, -45, 45) : Mathf.Clamp(RotationX, 0, 70);
            
            //Проверяем коллизию
            if (!IsAiming)
            {
                CollisionCheck(Player.transform.position, Rotation);

                if(!BackToStandartCameraProsition)
                {
                    //Преобразуем угол Еулера в кватернион.
                    Rotation = Quaternion.Euler(RotationX, RotationY, 0);
                }
            }
            else
            {
                //Преобразуем угол Еулера в кватернион.
                Rotation = Quaternion.Euler(RotationX, Player.eulerAngles.y, 0);
            }

            //Получаем нужное положение для камеры
            StandartCameraPosition = Player.transform.position - (Rotation * Offset);

            //Двигаем камеру
            CameraMove(Rotation);

            //Задаем направление камеры, если игрок не прицеливается
            if(!IsAiming)
            {
                Camera.transform.LookAt(Player.transform);
            }
        }

        /// <summary>
        /// Получаем данные из контроллера ввода
        /// </summary>
        /// <param name="inputController">Контроллер ввода</param>
        private void GetInputs()
        {
            //Проверяем нажата ли кнопка прицеливания
            IsAiming = inputController.Aim;

            //Получаем значения колесика мыши
            Zoom = inputController.Zoom;

            //Получаем значения движения мыши по оси X
            RotationX += inputController.RotationX * (CameraModel.AxisY_MouseSensivity * 2);

            if (!BackToStandartCameraProsition)
            {
                //Получаем значения движения мыши по оси Y
                RotationY += inputController.RotationY * (CameraModel.AxisX_MouseSensivity * 2);
            }
            else
            {
                RotationY = Camera.transform.eulerAngles.y;
                
            }
            

        }

        /// Метод для передвижения камеры
        /// </summary>
        /// <param name="CameraObstacle">Флаг препятствия</param>
        /// <param name="Rotation">Текущее вращение камеры</param>
        private void CameraMove(Quaternion Rotation)
        {
            switch(CameraObstacle & !IsAiming)
            {
                case true:

                    Camera.transform.position = Vector3.Lerp(Camera.transform.position, ObstacleCameraPosition, CameraModel.CameraObstacleAvoidSpeed * Time.deltaTime);
                    
                    break;

                case false:
                    
                    if (!IsAiming)
                    {
                        Camera.transform.position = Vector3.Lerp(Camera.transform.position, StandartCameraPosition, CameraModel.CameraReturnSpeed * Time.deltaTime);
                        if(BackToStandartCameraProsition & Vector3.Distance(Camera.transform.position, StandartCameraPosition) < 0.2f)
                        {
                            BackToStandartCameraProsition = false;
                        }
                    }

                    else
                    {
                        Camera.transform.rotation = Player.transform.rotation *  Quaternion.Euler(RotationX, 0, 0);
                        Camera.transform.position = Vector3.Lerp(Camera.transform.position, AimPosition.position, CameraModel.CameraReturnSpeed * Time.deltaTime);
                        BackToStandartCameraProsition = true;
                    }
                    
                    break;
            }

            //Управление зумом камеры.
            CameraZoom();
        }

        /// <summary>
        /// Метод проверки коллизий
        /// </summary>
        /// <param name="PlayerPosition">Позиция игрока</param>
        /// <param name="rotation">Вращение камеры</param>
        private void CollisionCheck(Vector3 PlayerPosition, Quaternion rotation)
        {
            Debug.DrawLine(PlayerPosition, (PlayerPosition - (rotation * Offset)), Color.yellow);

            //Проверяем столкновение луча с препятствием
            if (Physics.Linecast(PlayerPosition, (PlayerPosition - (rotation * Offset)), out Ray, CameraModel.Mask))
            {
                CameraObstacle = true;

                //Отдялаем новую позицию камеры от препятствия.
                ObstacleCameraPosition = Ray.point + (rotation * (Vector3.forward * CameraModel.DistanceFromObstacle));

                Debug.DrawLine(Player.transform.position, ObstacleCameraPosition, Color.red);

                return;
            }

            CameraObstacle = false;
        }

        /// <summary>
        /// Метод "зума" камеры
        /// </summary>
        /// <param name="MinDistance">Минимальная дистанция</param>
        /// <param name="MaxDistance">Максимальная дистанция</param>
        private void CameraZoom()
        {
            //Управление зумом камеры.
            if (Zoom != 0)
            {
                //Муняем зум на колесо мыши
                Offset.z -= (1 * Input.GetAxis("Mouse ScrollWheel")) * CameraModel.CameraZoomSpeed * Time.deltaTime;

                //Ограничиваем зум камеры.
                switch (CameraObstacle)
                {
                    case true:
                        float MaxObstacleDist = Vector3.Distance(Player.transform.position, Camera.transform.position);
                        Offset.z = Mathf.Clamp(Offset.z, CameraModel.CameraMinDistance, MaxObstacleDist);
                        break;

                    case false:
                        Offset.z = Mathf.Clamp(Offset.z, CameraModel.CameraMinDistance, CameraModel.CameraMaxDistance);
                        break;
                }
            }
        }

        public override void ControllerUpdate()
        {

        }
    }
}
