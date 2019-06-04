using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(NPCPatrolController))]
[RequireComponent(typeof(NPCIdleController))]
[RequireComponent(typeof(RouteCompile))]
[RequireComponent(typeof(NPCMove))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(EnemyChase))]
public class EnemyController : MonoBehaviour
{
    bool alive;
    bool onPatrol; //Находится ли enemy в патруле
    bool inChase; // Взаимодействует ли с ним игрок
    bool onIdle; // Бездействует ли enemy
    Vector3 startPosition;
    float patrolRange;
    Vector3[] route; //
    int patrolChance = 5;
    int idleChance = 5;
    GameObject player;

    MeshRenderer mesh;

    private void Awake()
    {
        alive = true;
        onPatrol = false;
        onIdle = false;
        inChase = false;
        startPosition = transform.position;
        patrolRange = 15;
        NPCPatrolController.PatrolEvent += PatrolWaiter;
        NPCIdleController.IdleEvent += IdleWaiter;
        EnemyChase.ChaseEvent += FinishChase;
        EnemyDie.DieEvent += DestroyUnit;
        player = GameObject.FindGameObjectWithTag("Player");
        mesh = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        if (alive)
        {
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
            GetComponent<EnemyChase>().StopChase();
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<EnemyDie>().Die(mesh);
        }
    }

    private void DisableAll()
    {
        onIdle = false;
        onPatrol = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            inChase = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            alive = false;
        }
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