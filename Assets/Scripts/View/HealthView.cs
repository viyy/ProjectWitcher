using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using UnityEngine.UI;
namespace Assets.Scripts.Views
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image _HealthScaleImage;
        [SerializeField] private float _currentHealthPercent;
        [SerializeField] private float _currentHealth;
        private HealthModel healthModel;

        private void Awake()
        {
            healthModel = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthModel>();
            _HealthScaleImage = GetComponent<Image>();
        }


        void Update()
        {
            _currentHealth = healthModel.health;
            HealthShow();
        }
        private void HealthShow()
        {
            _currentHealthPercent = _currentHealth * 0.01f;
            _HealthScaleImage.fillAmount = _currentHealthPercent; ;
        }
    }
}