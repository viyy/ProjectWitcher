using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaView : MonoBehaviour
{
    [SerializeField] private Image _scaleImage;
    [SerializeField] private float _currentStaminaPercent;
    [SerializeField] private float _currentStamina;
    [SerializeField] private UnitModel _unitModel;

    void Awake()
    {
        _unitModel = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitModel>();
        _scaleImage = GetComponent<Image>();
        
    }

   
    void Update()
    {
        _currentStamina = _unitModel._currentStamina;
        StaminaShow();
    }
    private void StaminaShow()
    {
        _currentStaminaPercent = _currentStamina * 0.01f;
        _scaleImage.fillAmount = _currentStaminaPercent;
    }
}
