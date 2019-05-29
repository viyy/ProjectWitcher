using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaController : MonoBehaviour
{
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _currentStamina;
    [SerializeField] private UnitModel _unitModel;
    [SerializeField] private MoveState _moveState;
    private float _currentStaminaRunCoast = 0.5f;
    private float _staminaRegenRate=10;


    private void Awake()
    {
        _unitModel = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitModel>();
        _maxStamina = _unitModel._maxStamina;
    }
    private void Start()
    {
        _currentStamina = _maxStamina;
    }


    void Update()
    {
        StaminaSpendingForRun();
        StaminaRegeneration();
        
        _moveState = _unitModel._moveState;
        _unitModel._currentStamina = _currentStamina;

    }

    private void StaminaSpendingForRun()
    {
        if (_moveState == MoveState.run)
        {
            _currentStamina -= _currentStaminaRunCoast;
            if (_currentStamina <= 0) _currentStamina = 0;
        }

    }

    // Регенерация стамины
    private void StaminaRegeneration()
    {
        if (_moveState == MoveState.walk || _moveState == MoveState.stand)
        {
            _currentStamina += _staminaRegenRate * Time.deltaTime;
        }

        if (_currentStamina >= _maxStamina)
        {
            _currentStamina = _maxStamina;
        }
    }

}
