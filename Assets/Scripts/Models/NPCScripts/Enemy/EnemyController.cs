using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(NPCPatrolController))]
[RequireComponent(typeof(NPCIdleController))]
[RequireComponent(typeof(RouteCompile))]
[RequireComponent(typeof(NPCMove))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(EnemyChase))]
[RequireComponent(typeof(EnemyDie))]
public class EnemyController : MonoBehaviour, ISetDamage
{
    float hp;
    bool alive; //жив ли враг
    bool onPatrol; //Находится ли enemy в патруле
    bool inChase; // Взаимодействует ли с ним игрок
    bool onIdle; // Бездействует ли enemy
    Vector3 startPosition; //стартовая позиция для генерации зоны патрулирования
    public float patrolRange = 15f; //радиус зоны патрулирования
    Vector3[] route; //маршрут точек для патруля
    int patrolChance = 5;//шанс выбора режима патрулирования
    int idleChance = 5;//шанс выбора режима бездействия
    GameObject player;//игрок

    MeshRenderer mesh;

    private void Awake()
    {
        hp = 100;
        alive = true;
        onPatrol = false;
        onIdle = false;
        inChase = false;
        startPosition = transform.position; //загружаем в стартовую позицию начальное положение врага
        ///<summary>
        ///подписка на события
        /// </summary>
        NPCPatrolController.PatrolEvent += PatrolWaiter;
        NPCIdleController.IdleEvent += IdleWaiter;
        EnemyChase.ChaseEvent += FinishChase;
        EnemyDie.DieEvent += DestroyUnit;
        ///<summary>
        ///находим объект игрока
        /// </summary>
        player = GameObject.FindGameObjectWithTag("Player");
        mesh = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            ///<summary>
            ///Проверка состояния деятельности врага
            /// </summary>
            if (inChase)
            {
                DisableAll();
                GetComponent<EnemyChase>().Chase(startPosition, patrolRange, player);
                Debug.Log("Chasing");
            }
            else if (onPatrol)
            {
                GetComponent<NPCPatrolController>().Patrol(route);
            }
            else if (onIdle)
            {

            }
            else
            {
                int choseAct = Random.Range(-patrolChance, idleChance);
                if (choseAct < 0)
                {
                    onPatrol = true;
                    route = GetComponent<RouteCompile>().Compile(startPosition, patrolRange);
                    patrolChance--;
                    idleChance = 5;
                }
                else
                {
                    onIdle = true;
                    GetComponent<NPCIdleController>().Idle();
                    idleChance--;
                    patrolChance = 5;
                }
            }
        }
        else
        {
            GetComponent<EnemyChase>().StopChase();//останавливаем погоню
            GetComponent<CapsuleCollider>().enabled = false;//выключаем коллайдер
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;//замораживаем перемещения и повороты
            GetComponent<EnemyDie>().Die(mesh);//запускаем событие смерти
        }
    }

    /// <summary>
    /// Отключаем состояния бездействия и патрулирования для начала погони
    /// </summary>
    private void DisableAll()
    {
        onIdle = false;
        onPatrol = false;
    }


    /// <summary>
    /// запускаем погоню, если игрок попал в зону триггера
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            inChase = true;
        }
    }

    /// <summary>
    /// Убиваем врага, если он столкнулся с игроком
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            
        }
    }

    public void ApplyDamage(float damage)
    {
        if(hp > damage)
        {
            hp = hp - damage;
        }
        else
        {
            hp = 0;
            alive = false;
        }
        Debug.Log(hp);
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject == player)
    //    {
    //        inChase = false;
    //    }
    //}

    /// <summary>
    /// Методы подписывающиеся на события для отслеживания состояний
    /// </summary>
    /// <param name="condition"></param>
    #region Subscribers
    private void FinishChase()
    {
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
    private void DestroyUnit()
    {
        Destroy(gameObject);
    }
    #endregion

}