using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;

namespace Assets.Scripts.Models
{

    public class HealthModel : MonoBehaviour, IDamageable
    {
        // Текущее количество жизни
        public float health = 80;

        // Максимальное количество жизни
        public float healthMaximum = 100;

        // Реген рейт хп
        public float healthRegenerationRate = 3;

        private ParticleSystem bloodSplash;
        private void Awake()
        {
            bloodSplash = GameObject.FindGameObjectWithTag("Player").GetComponent<ParticleSystem>();
        }
        

        // Получение урона
        public void TakeDamage(float damage)
        {
            Debug.Log($"I was hitted for {damage} damage");
            health -= damage;
            bloodSplash.Play();
            
        }

        

    }
}