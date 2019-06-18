using UnityEngine;
using Assets.Scripts.BaseScripts;

public class AnimController : BaseController
{
    //Ссылка на аниматор
    private Animator _animator;
    //Ссылка на модель игрока
    private GameObject player;

    //Состояния передающиеся в параметры анимаций (Состояния анимаций)
    public bool roll { get; private set; }
    public bool jump { get; private set; }
    public bool run { get; private set; }
    public bool defence { get; private set; }
    public bool normaAttack { get; private set; }
    public bool heavyAttack { get; private set; }
    public bool aiming { get; private set; }
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
   
    //Ссылка на параметры анимаций
    public AnimationsParametorsModel animationsParametorsModel;

    //Конструктор
    public AnimController(GameObject player)
    {
        this.player = player;      
        animationsParametorsModel = new AnimationsParametorsModel();
        _animator = player.GetComponent<Animator>();
    }

    public override void ControllerUpdate()
    {
        //Вызов метода проверки состояний игрока 
        GetInputs();

        //Вызов метода проигрывания анимаций игрока
        PlayAnimations();
    }

    //Метод Проигрывания Анимаций
    private void PlayAnimations()
    {
        //Горизонтальный ввод клавиатуры
        _animator.SetFloat(animationsParametorsModel.horizontal, horizontal);

        //Вертикальный ввод клавиатуры
        _animator.SetFloat(animationsParametorsModel.vertical, vertical);

        //Проверка на Прыжок 
        if (jump)
        {
            _animator.SetBool(animationsParametorsModel.isJumping, true);
        }
        else
        {
            _animator.SetBool(animationsParametorsModel.isJumping, false);
        }

        //Проверка на Бег
        if (run)
        {
            _animator.SetBool(animationsParametorsModel.isRuning, true);
        }
        else
        {
            _animator.SetBool(animationsParametorsModel.isRuning, false);
        }
        

        //Проверка на Кувырок 
        if (roll)
        {
            _animator.SetBool(animationsParametorsModel.isRolling, true);
        }
        else
        {
            _animator.SetBool(animationsParametorsModel.isRolling, false);
        }

        //Проверка на Обычный удар
        if (normaAttack)
        {
            _animator.SetBool(animationsParametorsModel.isNormalAttack, true);
        }
        else
        {
            _animator.SetBool(animationsParametorsModel.isNormalAttack, false);
        }

        //Проверка на Тяжелый удар
        if (heavyAttack)
        {
            _animator.SetBool(animationsParametorsModel.isHeavyAttack, true);
        }
        else
        {
            _animator.SetBool(animationsParametorsModel.isHeavyAttack, false);
        }

        if (defence)
        {
            _animator.SetBool(animationsParametorsModel.isDefence, true);
            Debug.Log("Defence");
        }
        else
        {
            _animator.SetBool(animationsParametorsModel.isDefence, false);
        }

        if (aiming)
        {
            _animator.SetBool(animationsParametorsModel.isAiming, true);
        }
        else
        {
            _animator.SetBool(animationsParametorsModel.isAiming, false);
        }
    }

    //Метод проверки состояний различных состояний игрока
    private void GetInputs()
    {
        horizontal = StartScript.GetStartScript.inputController.ForwardBackward;
        vertical = StartScript.GetStartScript.inputController.LeftRight;
        defence = StartScript.GetStartScript.inputController.Defence;
        jump = StartScript.GetStartScript.staminaController.CanJump;
        run = StartScript.GetStartScript.staminaController.CanRun;
        roll = StartScript.GetStartScript.staminaController.CanRoll;
        normaAttack = StartScript.GetStartScript.staminaController.CanNormalAttack;
        heavyAttack = StartScript.GetStartScript.staminaController.CanHeavyAttack;
        aiming = StartScript.GetStartScript.inputController.Aim;
    }
}