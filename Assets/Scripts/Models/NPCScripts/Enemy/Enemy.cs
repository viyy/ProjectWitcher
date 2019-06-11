using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySpace
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour, ISetDamage
    {
        public delegate void SeeEnemy();
        public static event SeeEnemy SeeEvent;
        public delegate void TakeDamage(float dmg);
        public static event TakeDamage DamageEvent;

        EnemyController controller;
        [SerializeField] EnemySpecifications specification;
        Transform _transform;
        MeshRenderer mesh;
        NavMeshAgent agent;
        Rigidbody rb;
        CapsuleCollider enemyBorder;
        SphereCollider enemyView;
        GameObject player;
        public void EnemyAwake()
        {
            _transform = GetComponent<Transform>();
            mesh = GetComponent<MeshRenderer>();
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            enemyBorder = GetComponent<CapsuleCollider>();
            enemyView = GetComponent<SphereCollider>();
            player = GameObject.FindGameObjectWithTag("Player");
            enemyView.radius = specification.ViewDistance;
            controller = new EnemyController(_transform, agent, mesh, rb, enemyBorder, enemyView, specification, _transform.position, player);
            controller.EnemyControllerAwake();
        }
        public void EnemyUpdate(float deltaTime)
        {
            controller.EnemyControllerUpdate(deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == player)
            {
                SeeEvent();
            }
            
        }

        public void ApplyDamage(float damage)
        {
            DamageEvent(damage);
        }
    }
}

