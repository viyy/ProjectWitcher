using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySpace
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Enemy : MonoBehaviour, ISetDamage
    {
        public delegate void SeeEnemy();
        public static event SeeEnemy SeeEvent;
        public delegate void TakeDamage(float dmg);
        public static event TakeDamage DamageEvent;

        EnemyController controller;
        [SerializeField] EnemySpecifications specification;
        Transform _transform;
        Transform head;
        MeshRenderer gun;
        MeshRenderer knife;
        Transform gunBarrelEnd;
        MeshRenderer mesh;
        MeshRenderer headMesh;
        NavMeshAgent agent;
        Rigidbody rb;
        CapsuleCollider enemyBorder;
        SphereCollider enemyView;
        LineRenderer shootLine;
        GameObject player;
        public void EnemyAwake()
        {
            _transform = GetComponent<Transform>();
            head = _transform.GetChild(0);
            mesh = GetComponent<MeshRenderer>();
            headMesh = head.GetComponent<MeshRenderer>();
            gun = _transform.GetChild(1).GetComponent<MeshRenderer>();
            knife = _transform.GetChild(3).GetComponent<MeshRenderer>();
            knife.enabled = false;
            gunBarrelEnd = _transform.GetChild(2);
            headMesh.material.color = new Color(1, 0.9058824f, 0.6745098f, 1);
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            enemyBorder = GetComponent<CapsuleCollider>();
            enemyView = GetComponent<SphereCollider>();
            shootLine = GetComponentInChildren<LineRenderer>();
            player = GameObject.FindGameObjectWithTag("Player");
            enemyView.radius = specification.ViewDistance;
            controller = new EnemyController(_transform, agent, mesh, headMesh, gun, knife, gunBarrelEnd, rb, enemyBorder, enemyView, shootLine, specification, _transform.position, player);
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

