using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel : MonoBehaviour
{
    // Жизнь
    public float _maxHealth;
    public float _currentHealth;

    // Мана
    public float _maxMana;
    public float _currentMana;

    // Стамина
    public float _maxStamina = 100;
    public float _currentStamina;
    public float _staminaRegenRate = 10;
    // Скорость для расчета хотьбы и бега, в случае ее изменения от умений\эффектов\предметов
    public float _speed;

    // Уровень персонажа и полученный опыт
    public float _level;
    public float _expiriens;

    // Коэффициенты для расчета нанесенного и получаемого урона.
    public float _attackRate;
    public float _defenceRate;
    
    // Флаг регенерации
    public bool _isRegeneration;

    // Текущие состояние персонажа (движение)
    public MoveState _moveState;

    // Текущее боевое состояние
    public FightState _fightState;

    public bool _canRun;

    public bool _canWalk = true;

    public bool _canJump;

    public bool _canSit;

    public bool _canStand = true;


    // Смерть
    public bool _isDead = false;

    /// <summary>
    /// Инвентарь
    /// public Inventory _inventory;
    /// </summary>


    private void Update()
    {
        StaminaCheckForMove();
    }

    // Методы для обработки стамины
    #region Stamina's methods


    //// Регенерация стамины
    //private void StaminaRegeneration()
    //{
    //    if (_moveState == MoveState.walk)
    //    {
    //        _isRegeneration = true;
    //        _currentStamina += _staminaRegenRate;
    //    }

    //    if (_currentStamina >= _maxStamina)
    //    {
    //        _currentStamina = _maxStamina;
    //    }
    //}
    // Проверка на остаток стамины
    private void StaminaCheckForMove()
    {
        if (_currentStamina <= 1)
        {
            _canRun = false;
            _canJump = false;
        }
        else
        {
            if (_currentStamina >= 20) 
            _canRun = true;
            _canJump = true;
        }
    }
    public void ChangeMoveState(float n)
    {
        if (n == 0)
        {
            _moveState = MoveState.stand;
        }
        if (n == 1)
        {
            _moveState = MoveState.walk;
        }
        if (n == 2)
        {
            _moveState = MoveState.run;
        }
        if (n == 3)
        {
            _moveState = MoveState.sit;
        }
        if (n == 4)
        {
            _moveState = MoveState.jump;
        }
    }

    #endregion
}

