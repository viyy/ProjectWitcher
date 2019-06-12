using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

namespace EnemySpace
{
    public class EnemyController
    {
        private string type;
        private float hp;
        public float CurrentHP { get; private set; }
        private float speed;
        private float runSpeed;
        Vector3 homePoint; //стартовая позиция для генерации зоны патрулирования
        float patrolDistance; //радиус зоны патрулирования
        float chasingTime;
        float rangeDistance;
        float rangeDamage;
        float rangeAccuracy;
        float shootSpeed;
        float meleeDistance;

        float timer = 0f;

        /// <summary>
        /// Состояния
        /// </summary>
        #region Conditions
        bool alive; //жив ли враг
        bool onPatrol; //Режим патрулирования
        bool inChase; // Режим погони
        bool onIdle; // Режим бездействия
        bool inFight; //Режим сражения
        bool comingHome;
        #endregion


        Vector3[] route; //маршрут точек для патруля   

        /// <summary>
                         /// Шансы выбора действия
                         /// </summary>
        #region Chances
        int patrolChance = 5;//шанс выбора режима патрулирования
        int idleChance = 5;//шанс выбора режима бездействия
        #endregion

        /// <summary>
        /// Дополнительно подключаемые компоненты, не наследованные от монобеха
        /// </summary>
        #region Instance
        RouteCompile RouteGenerator;
        EnemyDie Dying;
        EnemyMove Move;
        EnemyChase Chasing;
        EnemyPatrolController Patroling;
        EnemyIdleController Idle;
        EnemyComingHome ComingHome;
        EnemyFightController Fight;
        EnemyHurt Hurt;
        #endregion

        /// <summary>
        /// Кэшированные компоненты
        /// </summary>
        #region Cache
        MeshRenderer mesh;
        MeshRenderer headMesh;
        Transform enemyTransform;
        MeshRenderer gun;
        MeshRenderer knife;
        Transform gunBarrelEnd;
        NavMeshAgent agent;
        Rigidbody rb;
        CapsuleCollider enemyBorder;
        SphereCollider enemyView;
        LineRenderer shootLine;
        GameObject player;//игрок
        #endregion

        public EnemyController(Transform enemyTransform, NavMeshAgent agent, MeshRenderer mesh, MeshRenderer headMesh, MeshRenderer gun, MeshRenderer knife, Transform gunBarrelEnd, Rigidbody rb, CapsuleCollider enemyBorder, SphereCollider enemyView, LineRenderer shootLine, EnemySpecifications spec, Vector3 homePoint, GameObject player)
        {
            this.enemyTransform = enemyTransform;
            this.agent = agent;
            this.mesh = mesh;
            this.headMesh = headMesh;
            this.enemyBorder = enemyBorder;
            this.enemyView = enemyView;
            this.rb = rb;
            this.shootLine = shootLine;
            this.hp = spec.HP;
            this.speed = spec.Speed;
            this.runSpeed = spec.RunSpeed;
            this.chasingTime = spec.ChasingTime;
            this.homePoint = homePoint;
            this.patrolDistance = spec.PatrolDistance;
            this.player = player;
            this.type = spec.Type;
            this.rangeDistance = spec.RangeDistance;
            this.rangeDamage = spec.RangeDamage;
            this.rangeAccuracy = spec.RangeAccuracy;
            this.shootSpeed = spec.ShootSpeed;
            this.meleeDistance = spec.MeleeDistance;
            this.gun = gun;
            this.knife = knife;
            this.gunBarrelEnd = gunBarrelEnd;
        }

        public void EnemyControllerAwake()
        {
            alive = true;
            onPatrol = false;
            onIdle = false;
            inChase = false;
            inFight = false;
            comingHome = false;

            CurrentHP = hp;
            
            RouteGenerator = new RouteCompile();
            Dying = new EnemyDie(enemyTransform);
            Move = new EnemyMove(agent, speed, rb);
            if(type == "Range")
            {
                Chasing = new EnemyChase(Move, enemyTransform, runSpeed, chasingTime, rangeDistance);
            }
            else if(type == "Melee")
            {
                Chasing = new EnemyChase(Move, enemyTransform, runSpeed, chasingTime, meleeDistance);
            }           
            Patroling = new EnemyPatrolController(Move, enemyTransform);
            Idle = new EnemyIdleController();
            ComingHome = new EnemyComingHome(Move, enemyTransform, homePoint);
            if(type == "Range")
            {
                Fight = new EnemyFightController(Move, enemyTransform, gun, knife, gunBarrelEnd, shootLine, rangeDistance, meleeDistance, runSpeed, rangeDamage, rangeAccuracy, shootSpeed);
            }
            if (type == "Melee")
            {
                Fight = new EnemyFightController(Move, enemyTransform, gun, knife, gunBarrelEnd, shootLine, meleeDistance, rangeDistance, runSpeed, rangeDamage, rangeAccuracy, shootSpeed);
            }
            Hurt = new EnemyHurt(headMesh);

            ///<summary>
            ///подписка на события
            /// </summary>
            EnemyPatrolController.PatrolEvent += PatrolWaiter;
            EnemyIdleController.IdleEvent += IdleWaiter;
            EnemyChase.ChaseEvent += FinishChase;
            EnemyChase.AttackSwitchEvent += AttackMode;
            Enemy.SeeEvent += StartChase;
            Enemy.DamageEvent += TakeDamage;
            EnemyComingHome.ComingHomeEvent += AtHome;

        }

        public void EnemyControllerUpdate(float deltaTime)
        {
            if (alive)
            {
                ///<summary>
                ///Проверка состояния деятельности врага
                /// </summary>
                if (inChase)
                {
                    enemyView.enabled = false;
                    Chasing.Chase(player, deltaTime);
                    Debug.Log("Chasing");
                }
                else if (comingHome)
                {
                    Debug.Log("ComingHome");
                    timer += deltaTime;
                    ComingHome.ComingHome();
                    if(timer > 3f && !enemyView.enabled)
                    {
                        Debug.Log("ViewEnabled");
                        enemyView.enabled = true;
                    }
                }
                else if (onPatrol)
                {
                    Patroling.Patrol(route);
                }
                else if (onIdle)
                {
                    Idle.Idle();
                }
                else if (inFight)
                {
                    Fight.Fight(player, deltaTime);
                }
                else
                {
                    int choseAct = Random.Range(-patrolChance, idleChance);
                    if (choseAct < 0)
                    {
                        onPatrol = true;
                        route = RouteGenerator.Compile(homePoint, patrolDistance);
                        patrolChance--;
                        idleChance = 5;
                    }
                    else
                    {
                        onIdle = true;
                        idleChance--;
                        patrolChance = 5;
                    }
                }
            }
            else
            {
                Move.Stop();
                headMesh.enabled = false;
                gun.enabled = false;
                knife.enabled = false;
                enemyBorder.enabled = false;//выключаем коллайдер
                rb.constraints = RigidbodyConstraints.FreezeAll;//замораживаем перемещения и повороты
                Dying.Die(mesh, deltaTime);//запускаем событие смерти
            }
        }

        /// <summary>
        /// Методы подписывающиеся на события для отслеживания состояний
        /// </summary>
        /// <param name="condition"></param>
        #region Subscribers
        private void FinishChase()
        {
            inChase = false;
            comingHome = true;
            timer = 0f;
        }
        private void AtHome()
        {
            comingHome = false;
        }
        private void AttackMode()
        {
            inFight = true;
            inChase = false;
        }
        private void PatrolWaiter()
        {
            onPatrol = false;
        }
        private void IdleWaiter()
        {
            onIdle = false;
        }
        private void StartChase()
        {
            inChase = true;
            onIdle = false;
            onPatrol = false;
        }
        private void TakeDamage(float dmg)
        {
            if(CurrentHP > dmg)
            {
                CurrentHP = CurrentHP - dmg;
                float lifePercent = CurrentHP / hp * 100;
                Hurt.Hurt(lifePercent);
            }
            else
            {
                CurrentHP = 0;
                alive = false;
            }
        }
        #endregion

    }
}
