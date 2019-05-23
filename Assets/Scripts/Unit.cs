using UnityEngine;
using UnityEngine.UI;

public class Unit: MonoBehaviour
{
    //Использование стамины
    public void DrainStamina()
    {
        StaminaSpendingForRun();
    }

    private void Awake()
    {
        //Awake Скрипта Стамины
        #region StaminaAwake
        _charStats = FindObjectOfType<CharacterStats>();
        _image = GetComponent<Image>();
        _moveStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementController>(); // кэш MovementController, поиск по тегу Player
        _MaxStamina = _charStats._MaxStamina;
        _stamina = _MaxStamina;
        #endregion
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        //Update Скрипта Стамины
        #region Stamina Update
        StaminaShow();
        MovementStatusCheck();
        StaminaRegeneration();


        if (!_isRunning)
        {
            _isRegeneration = true;
        }
        else
        {
            _isRegeneration = false;
        }


        if (_isRunning)
        {
            StaminaSpendingForRun();
        }
        #endregion
    }

    //Скрипт Стамины
    #region StaminaUsing
    [SerializeField] private CharacterStats _charStats;
    [SerializeField] private Image _image;
    [SerializeField] private MovementController _moveStatus;
    [SerializeField] private float _staminaPercent;
    [SerializeField] private float _staminaConst = 100;
    private float _staminaRunCoast = 0.5f;
    private float _staminaJumpCoast = 30f;
    private float _staminaRegenRate = 0.1f;
    [SerializeField] private float _MaxStamina;
    [SerializeField] private float _stamina;

    public bool _isRunning = false;
    public bool _isJumping = false;
    public bool _isRegeneration = false;
    public bool _isStanding = true;

    private void StaminaShow()
    {
        _staminaPercent = _stamina * 0.01f;
        _image.fillAmount = _staminaPercent;
    }
    private void StaminaSpendingForRun()
    {
        _stamina = _stamina - _staminaRunCoast;
        if (_stamina <= 0)
        {
            _isRunning = false;
            _stamina = 0;
        }
    }

    //реген стамины

    private void StaminaRegeneration()
    {
        if (_isStanding & _isRegeneration)
            _stamina += _staminaRegenRate;
        if (_stamina >= _MaxStamina)
        {
            _stamina = _MaxStamina;
            _isRegeneration = false;
        }
    }

    // для MovementController проверка на остаток стамины

    public bool StaminaChekForRun()
    {
        if (_stamina >= 1) return true;
        else return false;
    }

    // проверка нажата ли кнопка бега(и др. состояний) в MovementController
    private void MovementStatusCheck()
    {
        _isRunning = _moveStatus.RunCheck();
        _isJumping = _moveStatus.JumpCheck();
        _isStanding = _moveStatus.StandCheck();   // нужна проверка на Standing в MovementController
    }
    #endregion
}
