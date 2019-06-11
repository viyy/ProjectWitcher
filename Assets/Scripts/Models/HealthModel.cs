using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

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

        

        // Получение урона
        public void TakeDamage(float damage)
        {
            Debug.Log($"I was hitted for {damage} damage");
            health -= damage;
        }

        

    }
}