using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySpace
{
    public class EnemyMove
    {
        private float speed;
        private NavMeshAgent agent;
        private Rigidbody rb;

        public EnemyMove(NavMeshAgent agent, float speed, Rigidbody rb)
        {
            this.agent = agent;
            this.speed = speed;
            this.rb = rb;
        }

        public void Move(Vector3 direction)
        {
            agent.speed = speed;
            agent.SetDestination(direction);
        }
        public void Move(Vector3 direction, float speed)
        {
            agent.speed = speed;
            agent.SetDestination(direction);
        }
        public void Rotate(Vector3 direction)
        {
            direction.y = 0f;
            direction = direction.normalized;
            Quaternion newRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(newRotation);
        }
        public void Stop()
        {
            agent.isStopped = true;
        }
        public void Continue()
        {
            agent.isStopped = false;
        }
    }
}
