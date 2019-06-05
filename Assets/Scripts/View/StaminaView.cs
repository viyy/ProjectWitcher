using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.BaseScripts;
using Assets.Scripts.Models;
namespace Assets.Scripts.Views
{
    public class StaminaView : MonoBehaviour
    {
        [SerializeField] private Image _scaleImage;
        [SerializeField] private float _currentStaminaPercent;
        [SerializeField] private float _currentStamina;
        private StaminaModel staminaModel;
        

        void Awake()
        {
            staminaModel = GameObject.FindGameObjectWithTag("Player").GetComponent<StaminaModel>();
            _scaleImage = GetComponent<Image>();
        }


        void Update()
        {
            _currentStamina = staminaModel.Stamina;
            StaminaShow();
        }
        private void StaminaShow()
        {
            _currentStaminaPercent = _currentStamina * 0.01f;
            _scaleImage.fillAmount = _currentStaminaPercent;
        }
    }
}