using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.BaseScripts;
using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.Controllers
{
    class MovementController : BaseController
    {
        //Вектор движения персонажа
        private Vector3 Movement;

        //Ссылка на компонент Transform игрока
        private Transform Player;

        //Ссылка на камеру игрока
        private Transform Camera;

        //Ccылка на контроллер ввода
        PCInputController inputController;

        //Модель передвижения игрока
        PlayerMovement PlayerMovement;

        //Контроллер персонажа !!! Временное решение !!!
        protected CharacterController CharacterController;

        #region Флаги состояний

        public bool IsRunning { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsStanding { get; private set; }

        #endregion

        //Позиция персонажа по оси X
        private float X;

        //Позиция персонажа по оси Z
        private float Z;

        //Позиция персонажа по оси Y
        private float Y;

        //Кватернион для сохранения вращения камеры.
        Quaternion TempCameraRotation;

        //Кватернион для вращения персонажа в режиме прицеливания.
        Quaternion CharacterAimRotation;

        //Кватернион для направления движения.
        Quaternion Direction;

        //Структура для получения информации о столкновении луча с объектом.
        private RaycastHit RayHit;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlayerMovement">Модель передвижения игрока</param>
        /// <param name="Player">компонент Transform игрока</param>
        /// <param name="CharacterController">Контроллер персонажа</param>
        public MovementController(PlayerMovement PlayerMovement, Transform Player, CharacterController CharacterController)
        {
            //Cоздаем вектор движения
            Movement = new Vector3();

            //Получаем позицию игрока
            this.Player = Player;

            //Получаем позицию камеры
            Camera = UnityEngine.Camera.main.transform;

            //Получаем модель передвижения игрока
            this.PlayerMovement = PlayerMovement;

            //Получаем контроллер персонажа
            this.CharacterController = CharacterController;

            //Получаем ссылку на контроллер ввода.
            inputController = StartScript.GetStartScript.inputController;
        }

        public override void ControllerUpdate()
        {
            //Задаем нулевой вектор движения
            Movement = Vector3.zero;

            //Получаем данные с клавиатуры и мыши от контроллера ввода
            GetInputs(inputController);

            //Проверяем поверхность под игроком
            GroundCheck();

            //Проверяем стоит ли персонаж на месте
            IsStanding = IsGrounded & (Z == 0 & X == 0);

            Debug.Log("Standing: "+IsStanding);

            CharacterJump();

            //Если были нажаты клавиши то:
            if (!inputController.Aim)
            {
                CharacterMovement();
            }

            //Если зажата правая кнопка мыши
            else
            {
                AimCharacterMovement();
            }
        }

        /// <summary>
        /// Получаем данные из контроллера ввода
        /// </summary>
        /// <param name="inputController">Контроллер ввода</param>
        private void GetInputs(PCInputController inputController)
        {
            IsRunning = inputController.Run;

            // Check to see if the A or D key are being pressed
            X = Input.GetAxis("Horizontal") * (IsRunning ? PlayerMovement.RunSpeed : PlayerMovement.Speed);

            // Check to see if the W or S key is being pressed.  
            Z = Input.GetAxis("Vertical") * (IsRunning ? PlayerMovement.RunSpeed : PlayerMovement.Speed);
        }

        /// <summary>
        /// Метод для прыжка персонажа !!! На первое время, потом переделать !!!
        /// </summary>
        private void CharacterJump()
        {
            //Если игрок стоит на поверхности то можем прыгать.
            if (IsGrounded)
            {
                if (inputController.Jump)
                {
                    Y = PlayerMovement.JumpHeight;
                }
                else
                {
                    Y = PlayerMovement.MinimumFall;
                }
            }

            //Если игрок не стоит на поверхности, то проверяем соприкасается ли коллайдер персонажа с коллайдером под ногами.
            else
            {
                //Если игрок не соприкасается ни с чем, значит он падает или прыгает.
                if (!CharacterController.isGrounded)
                {
                    //Вертикальная скорость уменьшается с каждой секундой, пока не достигнет значения максимальной скорость падения. 
                    Y += (PlayerMovement.Gravity * 2) * Time.deltaTime;

                    if (Y < PlayerMovement.TerminalFall)
                    {
                        Y = PlayerMovement.TerminalFall;
                    }
                }
            }
        }

        /// <summary>
        /// Метод движения с регулированием вращения персонажа камерой
        /// </summary>
        /// <param name="Movement">Вектор движения</param>
        /// <param name="AxisX">Направление по оси X</param>
        /// <param name="AxisZ">Направление по оси Z</param>
        private void AimCharacterMovement()
        {
            //Добавляем скорость движения. Изменяем положение по осям x и z
            Movement.x = X;
            Movement.z = Z;

            //Ограничиваем скорость движения по диагонали.
            Movement = Vector3.ClampMagnitude(Movement, IsRunning ? PlayerMovement.RunSpeed : PlayerMovement.Speed);

            //Кватернион для сохранения текущего вращения камеры.
            TempCameraRotation = Camera.rotation;

            //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
            Camera.eulerAngles = new Vector3(0, Camera.eulerAngles.y, 0);

            //Сохраняем поворот камеры
            CharacterAimRotation = Camera.rotation;

            if (IsGrounded)
            {
                //Переводим локальные координаты вектора движения игрока в глобальные.
                Movement = Camera.TransformDirection(Movement);
            }
            else
            {
                //Прыгаем в направлении куда смотрит игрок. Можем регулировать дальность прыжка зажимаю WASD
                Movement = Player.forward * Movement.magnitude;
                Debug.DrawRay(Player.position, Player.forward, Color.red, 5f);
            }

            //Возвращаем поворот камеры.
            Camera.rotation = TempCameraRotation;

            //Вращаем персонажа
            if (IsGrounded)
            {
                Player.rotation = Quaternion.Lerp(Player.rotation, CharacterAimRotation, PlayerMovement.AimRotateSpeed * Time.deltaTime);
            }

            //Двигаем персонажа
            Movement.y = Y;

            //Двигаем персонажа
            CharacterController.Move(Movement * Time.deltaTime);
        }

        /// <summary>
        /// Метод движения персонажа
        /// </summary>
        /// <param name="Movement">Вектор движения</param>
        /// <param name="AxisX">Направление по оси X</param>
        /// <param name="AxisZ">Направление по оси Z</param>
        private void CharacterMovement()
        {
            //Добавляем скорость движения. Изменяем положение по осям x и z
            Movement.x = X;
            Movement.z = Z;

            //Ограничиваем скорость движения по диагонали.
            Movement = Vector3.ClampMagnitude(Movement, IsRunning ? PlayerMovement.RunSpeed : PlayerMovement.Speed);

            //Кватернион для сохранения текущего вращения камеры.
            TempCameraRotation = Camera.rotation;

            //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
            Camera.eulerAngles = new Vector3(0, Camera.eulerAngles.y, 0);
            
            if(IsGrounded)
            {
                //Переводим локальные координаты вектора движения игрока в глобальные.
                Movement = Camera.TransformDirection(Movement);
            }
            else
            {
                //Прыгаем в направлении куда смотрит игрок. Можем регулировать дальность прыжка зажимаю WASD
                Movement = Player.forward * Movement.magnitude;
                Debug.DrawRay(Player.position, Player.forward, Color.red, 5f);
            }
            
            //Возвращаем поворот камеры.
            Camera.rotation = TempCameraRotation;

            //Создаем кватернион направления движения, метод LookRotation() вычисляет кватернион который смотрит в направлении движения.
            Direction = Quaternion.LookRotation(Movement);

            //Вращаем персонажа
            if(Movement != Vector3.zero & IsGrounded)
            {
                Player.rotation = Quaternion.Lerp(Player.rotation, Direction, PlayerMovement.AimRotateSpeed * Time.deltaTime);
            }

            //Двигаем персонажа
            Movement.y = Y;

            CharacterController.Move(Movement * Time.deltaTime);
        }

        /// <summary>
        /// Метод проверки поверхности под игроком
        /// </summary>
        private void GroundCheck()
        {
            RayHit = new RaycastHit();

            //Делаем луч видимым
            Debug.DrawRay(Player.transform.position, (-Player.transform.up * PlayerMovement.GroundRayDistance), Color.green);

            IsGrounded = false;

            if(Y < 0 & Physics.Raycast(Player.transform.position, Vector3.down, out RayHit))
            {
                IsGrounded = RayHit.distance < PlayerMovement.GroundRayDistance;
            }

            Debug.Log("Ground: " +IsGrounded);
        }
    }
}
