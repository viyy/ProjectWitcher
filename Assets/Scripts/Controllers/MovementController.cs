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
        InputController inputController;

        //Контроллер персонажа !!! Временное решение !!!
        protected CharacterController CharacterController;

        #region Флаги состояний

        public bool IsRunning { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsStanding { get; private set; }
        public bool IsRolling { get; private set; }
        public bool IsWalking { get; private set; }
        public bool IsAiming { get; private set; }
        public bool IsDefencing { get; private set; }

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

        private float RollDistance;

        private float RotationY;

        //Переменная для задания скорости
        private float Speed;

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
            GetInputs();

            //Проверяем поверхность под игроком
            GroundCheck();

            //Проверяем стоит ли персонаж на месте
            IsStanding = IsGrounded & (Z == 0 & X == 0);

            IsWalking = IsGrounded & !IsRunning & (Z > 0 || X > 0);

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
        private void GetInputs()
        {
            Speed = PlayerMovement.Speed;

            IsAiming = StartScript.GetStartScript.inputController.Aim;

            RotationY = StartScript.GetStartScript.cameraController.YRotation;

            IsRunning = StartScript.GetStartScript.staminaController.CanRun;

            IsRolling = StartScript.GetStartScript.staminaController.CanRoll;

            IsDefencing = StartScript.GetStartScript.inputController.Defence;

            if (IsRunning & !IsAiming) Speed = PlayerMovement.RunSpeed;
            if (IsDefencing & !IsAiming) Speed = PlayerMovement.DefenceSpeed;

            // Check to see if the A or D key are being pressed
            X = Input.GetAxis("Horizontal") * Speed;
            // Check to see if the W or S key is being pressed.  
            Z = Input.GetAxis("Vertical") * Speed;
        }

        /// <summary>
        /// Метод для прыжка персонажа !!! На первое время, потом переделать !!!
        /// </summary>
        private void JumpAndGravity()
        {
            //Если игрок стоит на поверхности то можем прыгать.
            if(IsGrounded)
            {
                Y = StartScript.GetStartScript.staminaController.CanJump ? PlayerMovement.JumpHeight : PlayerMovement.MinimumFall;
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
            Movement = Vector3.ClampMagnitude(Movement, Speed);

            //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
            Camera.eulerAngles = new Vector3(0, Camera.eulerAngles.y, 0);

            if (IsGrounded)
            {
                //Переводим локальные координаты вектора движения игрока в глобальные.
                Movement = Camera.TransformDirection(Movement);
            }

            switch (IsAiming)
            {
                case true:

                    //Задаем направление вращения игрока как вращение камеры, если при прицеливанни игрок развернут в другую сторону
                    if(Quaternion.Dot(Player.rotation, Camera.rotation) < 0.9f)
                    {
                        Player.rotation = Camera.rotation;
                    }
                    

                    if (!IsGrounded)
                    {
                        //Прыгаем в направлении движения игрока. Можем регулировать дальность прыжка зажимаю WASD
                        Movement = CharacterController.velocity * Movement.normalized.magnitude;
                    }

                    //Вращаем персонажа если она на поверхности.
                    if (IsGrounded & !SpecialMove)
                    {
                        Player.rotation *= Quaternion.Euler(0, inputController.RotationY * PlayerMovement.AimRotateSpeed * Time.deltaTime, 0);
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
                        Player.rotation = Quaternion.Lerp(Player.rotation, Direction, PlayerMovement.RotateSpeed * Time.deltaTime);
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
        
        /// <summary>
        /// Метод проверки поверхности под игроком
        /// </summary>
        private void GroundCheck()
        {
            RayHit = new RaycastHit();

            //Делаем луч видимым
            Debug.DrawRay(Player.transform.position + CharacterController.center, (-CharacterController.transform.up * ((CharacterController.height / 2) + PlayerMovement.GroundRayDistanceOffset)), Color.yellow);

            if (Y < 0 & Physics.Raycast(Player.transform.position + CharacterController.center, Vector3.down, out RayHit))
            {
                IsGrounded = RayHit.distance < ((CharacterController.height / 2) + PlayerMovement.GroundRayDistanceOffset);
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
            if (IsRolling & !IsAiming)
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

            if(SpecialMove)
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
