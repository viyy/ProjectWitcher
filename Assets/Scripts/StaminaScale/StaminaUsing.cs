using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaminaUsing : MonoBehaviour
{
    [SerializeField] private CharacterStats _charStats;
    [SerializeField] private Image _image;
    [SerializeField] private MovementControllerFix _moveStatus;
    [SerializeField] private float _staminaPercent;
    [SerializeField] private float _staminaConst =100;
    private float _staminaRunCoast = 0.5f;
    private float _staminaJumpCoast = 30f;
    private float _staminaRegenRate = 0.1f;
    [SerializeField] private float _MaxStamina;
    [SerializeField] private float _stamina;

    public bool _isRunning;
    public bool _isJumping = false;
    public bool _isRegeneration = false;
    public bool _isStanding = true;
   

    // Start is called before the first frame update
    void Awake()
    {
        _charStats = gameObject.GetComponent<CharacterStats>(); 
        _image = GetComponent<Image>();
        _moveStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementControllerFix>(); // кэш MovementController, поиск по тегу Player
        _MaxStamina = _charStats._MaxStamina;
        _stamina = _MaxStamina;
       
    }

    // Update is called once per frame
    void Update()
    {

        StaminaShow();
        MovementStatusCheck();
        StaminaRegeneration();


        if (!_isRunning )
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
        
    }
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
        if(_isStanding & _isRegeneration)
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
        //_isJumping = MovementController.JumpCheck();
       // _isStanding = MovementController.StandCheck();   // нужна проверка на Standing в MovementController
    }
}
