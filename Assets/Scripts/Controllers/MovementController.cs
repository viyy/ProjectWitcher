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
        #region Модель

        //Модель передвижения игрока
        PlayerMovement PlayerMovement;

        #endregion

        //Вектор движения персонажа
        private Vector3 Movement;

        //Ссылка на компонент Transform игрока
        private Transform Player;

        //Ссылка на камеру игрока
        private Transform Camera;

        //Ccылка на контроллер ввода
        PCInputController inputController;

        //Контроллер персонажа !!! Временное решение !!!
        protected CharacterController CharacterController;

        #region Флаги состояний

        public bool IsRunning { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsStanding { get; private set; }
        public bool IsRolling { get; private set; }

        private bool SpecialMove = false;

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

        //Вектор для кувырка
        Vector3 EndPoint;
        
        //Вектор направления кувырка
        Vector3 RollDirection;

        //Позиция игрока до кувырка
        Vector3 PositionBeforRoll;

        float RollDistance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlayerMovement">Модель передвижения игрока</param>
        /// <param name="Player">компонент Transform игрока</param>
        /// <param name="CharacterController">Контроллер персонажа</param>
        public MovementController(Transform Player, CharacterController CharacterController)
        {
            //Cоздаем вектор движения
            Movement = new Vector3();

            //Получаем позицию игрока
            this.Player = Player;

            //Получаем позицию камеры
            Camera = UnityEngine.Camera.main.transform;

            //Создаем модель передвижения игрока
            PlayerMovement = new PlayerMovement();

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

            //Прыжки и гравитация
            JumpAndGravity();

            //Передвижение
            CharacterMove();
            
        }

        public override void ControllerLateUpdate()
        {
            Roll();
        }

        /// <summary>
        /// Получаем данные из контроллера ввода
        /// </summary>
        /// <param name="inputController">Контроллер ввода</param>
        private void GetInputs(PCInputController inputController)
        {
            IsRunning = StartScript.GetStartScript.staminaController.CanRun;

            IsRolling = StartScript.GetStartScript.staminaController.CanRoll;

            // Check to see if the A or D key are being pressed
            X = Input.GetAxis("Horizontal") * (IsRunning ? PlayerMovement.RunSpeed : PlayerMovement.Speed);

            // Check to see if the W or S key is being pressed.  
            Z = Input.GetAxis("Vertical") * (IsRunning ? PlayerMovement.RunSpeed : PlayerMovement.Speed);
        }

        /// <summary>
        /// Метод для прыжка персонажа !!! На первое время, потом переделать !!!
        /// </summary>
        private void JumpAndGravity()
        {
            //Если игрок стоит на поверхности то можем прыгать.
            if (IsGrounded)
            {
                if (StartScript.GetStartScript.staminaController.CanJump)
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
        /// Метод перемещения игрока
        /// </summary>
        private void CharacterMove()
        {
            if (!SpecialMove)
            {
                //Добавляем скорость движения. Изменяем положение по осям x и z
                Movement.x = X;
                Movement.z = Z;
            }

            //Ограничиваем скорость движения по диагонали.
            Movement = Vector3.ClampMagnitude(Movement, IsRunning ? PlayerMovement.RunSpeed : PlayerMovement.Speed);

            //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
            Camera.eulerAngles = new Vector3(0, Camera.eulerAngles.y, 0);

            if (IsGrounded)
            {
                //Переводим локальные координаты вектора движения игрока в глобальные.
                Movement = Camera.TransformDirection(Movement);
            }

            switch (inputController.Aim)
            {
                case true:

                    if (!IsGrounded)
                    {
                        //Прыгаем в направлении движения игрока. Можем регулировать дальность прыжка зажимаю WASD
                        Movement = CharacterController.velocity * Movement.normalized.magnitude;
                    }

                    //Сохраняем поворот камеры
                    CharacterAimRotation = Camera.rotation;

                    //Вращаем персонажа если она на поверхности.
                    if (IsGrounded & !SpecialMove)
                    {
                        Player.rotation = Quaternion.Lerp(Player.rotation, CharacterAimRotation, PlayerMovement.AimRotateSpeed * Time.deltaTime);
                    }

                    break;

                case false:

                    if (!IsGrounded)
                    {
                        //Прыгаем в направлении куда смотрит игрок. Можем регулировать дальность прыжка зажимаю WASD
                        Movement = Player.forward * Movement.magnitude;
                    }

                    //Создаем кватернион направления движения, метод LookRotation() вычисляет кватернион который смотрит в направлении движения.
                    if (Movement != Vector3.zero)
                    {
                        Direction = Quaternion.LookRotation(Movement);
                    }

                    //Вращаем персонажа если он двигается и он на поверхности
                    if (Movement != Vector3.zero & IsGrounded)
                    {
                        Player.rotation = Quaternion.Lerp(Player.rotation, Direction, PlayerMovement.AimRotateSpeed * Time.deltaTime);
                    }

                    break;
            }

            //Задаем направление по горизонтали
            Movement.y = Y;

            //Двигаем персонажа
            CharacterController.Move(Movement * Time.deltaTime);

            #region Debug

            Debug.DrawRay(Player.position, CharacterController.velocity.normalized, Color.blue, 1f);

            #endregion
        }

        #region Old Movement

        ///// <summary>
        ///// Метод движения с регулированием вращения персонажа камерой
        ///// </summary>
        ///// <param name="Movement">Вектор движения</param>
        ///// <param name="AxisX">Направление по оси X</param>
        ///// <param name="AxisZ">Направление по оси Z</param>
        //private void AimCharacterMovement()
        //{
        //    //Добавляем скорость движения. Изменяем положение по осям x и z
        //    Movement.x = X;
        //    Movement.z = Z;

        //    //Ограничиваем скорость движения по диагонали.
        //    Movement = Vector3.ClampMagnitude(Movement, IsRunning ? PlayerMovement.RunSpeed : PlayerMovement.Speed);

        //    //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
        //    Camera.eulerAngles = new Vector3(0, Camera.eulerAngles.y, 0);

        //    if (IsGrounded)
        //    {
        //        //Переводим локальные координаты вектора движения игрока в глобальные.
        //        Movement = Camera.TransformDirection(Movement);
        //    }
        //    else
        //    {
        //        //Прыгаем в направлении куда смотрит игрок. Можем регулировать дальность прыжка зажимаю WASD
        //        Movement = CharacterController.velocity * Movement.normalized.magnitude;
        //        Debug.DrawRay(Player.position, Player.forward, Color.green, 1f);
        //    }


        //    //Сохраняем поворот камеры
        //    CharacterAimRotation = Camera.rotation;

        //    //Вращаем персонажа
        //    if (IsGrounded)
        //    {
        //        Player.rotation = Quaternion.Lerp(Player.rotation, CharacterAimRotation, PlayerMovement.AimRotateSpeed * Time.deltaTime);
        //    }

        //    //Задаем направление по горизонтали
        //    Movement.y = Y;

        //    //Двигаем персонажа
        //    CharacterController.Move(Movement * Time.deltaTime);
        //}

        ///// <summary>
        ///// Метод движения персонажа
        ///// </summary>
        ///// <param name="Movement">Вектор движения</param>
        ///// <param name="AxisX">Направление по оси X</param>
        ///// <param name="AxisZ">Направление по оси Z</param>
        //private void CharacterMovement()
        //{
        //    //Добавляем скорость движения. Изменяем положение по осям x и z
        //    Movement.x = X;
        //    Movement.z = Z;

        //    //Ограничиваем скорость движения по диагонали.
        //    Movement = Vector3.ClampMagnitude(Movement, IsRunning ? PlayerMovement.RunSpeed : PlayerMovement.Speed);

        //    //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
        //    Camera.eulerAngles = new Vector3(0, Camera.eulerAngles.y, 0);

        //    if (IsGrounded)
        //    {
        //        //Переводим локальные координаты вектора движения игрока в глобальные.
        //        Movement = Camera.TransformDirection(Movement);
        //    }
        //    else
        //    {
        //        //Прыгаем в направлении куда смотрит игрок. Можем регулировать дальность прыжка зажимаю WASD
        //        Movement = Player.forward * Movement.magnitude;
        //        Debug.DrawRay(Player.position, Player.forward, Color.red, 0.1f);
        //    }

        //    //Создаем кватернион направления движения, метод LookRotation() вычисляет кватернион который смотрит в направлении движения.
        //    Direction = Quaternion.LookRotation(Movement);

        //    //Вращаем персонажа
        //    if (Movement != Vector3.zero & IsGrounded)
        //    {
        //        Player.rotation = Quaternion.Lerp(Player.rotation, Direction, PlayerMovement.AimRotateSpeed * Time.deltaTime);
        //    }

        //    //Двигаем персонажа
        //    Movement.y = Y;

        //    CharacterController.Move(Movement * Time.deltaTime);
        //}

        #endregion

        /// <summary>
        /// Метод проверки поверхности под игроком
        /// </summary>
        private void GroundCheck()
        {
            RayHit = new RaycastHit();

            //Делаем луч видимым
            Debug.DrawRay(Player.transform.position, (-Player.transform.up * PlayerMovement.GroundRayDistance), Color.yellow);

            if (Y < 0 & Physics.Raycast(Player.transform.position, Vector3.down, out RayHit))
            {
                IsGrounded = RayHit.distance < PlayerMovement.GroundRayDistance;
            }
            else
            {
                IsGrounded = false;
            }
        }

        /// <summary>
        /// Метод кувырка
        /// </summary>
        private void Roll()
        {
            if (IsRolling)
            {
                if (!IsStanding)
                {
                    EndPoint = RollPoint(Player.transform.position, CharacterController.velocity.normalized);
                }
                else
                {
                    EndPoint = RollPoint(Player.transform.position, Player.forward);
                }

                //Задаем направление
                RollDirection = EndPoint - Player.transform.position;

                //Запоминаем дистанцию для кувырка
                RollDistance = Vector3.Distance(Player.transform.position, EndPoint);

                //Запоминаем положение игрока до кувырка
                PositionBeforRoll = Player.transform.position;

                SpecialMove = true;
            }

            if (SpecialMove)
            {
                Debug.DrawRay(EndPoint, Vector3.up, Color.red, 15);
                
                //Проверяем пройденное расстояние
                if(Vector3.Distance(Player.transform.position, PositionBeforRoll) >= RollDistance)
                {
                    SpecialMove = false;
                    return;
                }

                CharacterController.Move(RollDirection * PlayerMovement.RollSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Метод для определения конечной позиции кувырка
        /// </summary>
        /// <param name="PlayerPosition">Текущая позация игрока</param>
        /// <param name="PointToMove">Предпологаемая конечная позиция кувырка</param>
        /// <returns>Конечную позицию кувырка с учетом препятствий</returns>
        private Vector3 RollPoint(Vector3 PlayerPosition, Vector3 PlayerDirection)
        {
            RaycastHit Hit = new RaycastHit();

            Vector3 ResultPoint = Vector3.zero;

            Vector3 StartPoint = new Vector3(PlayerPosition.x, (PlayerPosition.y - ((CharacterController.height / 2) - (CharacterController.stepOffset - 0.1f))), PlayerPosition.z);
            Vector3 EndPoint = StartPoint + (PlayerDirection * PlayerMovement.RollDistance);

            if (Physics.Linecast(StartPoint, EndPoint, out Hit))
            {
                ResultPoint = PlayerPosition + (PlayerDirection * (Hit.distance - (CharacterController.radius*2)));
                return ResultPoint;
            }

            ResultPoint = PlayerPosition + (PlayerDirection * PlayerMovement.RollDistance);
            return ResultPoint;
        }
    }
}
