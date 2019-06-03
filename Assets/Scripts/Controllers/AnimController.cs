using UnityEngine;
using Assets.Scripts.BaseScripts;

public class AnimController : BaseController
{
    //
    private Animator _animator;
    private GameObject player;
    private bool roll;
    private bool jump;
    private bool run;
    private bool normaAttack;
    private bool heavyAttack;
    private float horizontal;
    private float vertical;

    public PlayerAttackModel playerAttackModel;
    public AnimationsParametorsModel animationsParametorsModel;

    public AnimController(GameObject player)
    {
        this.player = player;
        playerAttackModel = new PlayerAttackModel();
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

    #region Метод Проигрывания Анимаций
    private void PlayAnimations()
    {
        _animator.SetFloat(animationsParametorsModel.horizontal, horizontal);
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
            _animator.SetBool(animationsParametorsModel.isJumping, false);
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
            _animator.SetBool(animationsParametorsModel.isHeavyAttack, false);
        }
    }
    #endregion

    //Метод проверки состояний различных состояний игрока
    private void GetInputs()
    {
        horizontal = StartScript.GetStartScript.inputController.ForwardBackward;
        vertical = StartScript.GetStartScript.inputController.LeftRight;
        jump = StartScript.GetStartScript.inputController.Jump;
        run = StartScript.GetStartScript.inputController.Run;
        roll = StartScript.GetStartScript.inputController.Roll;
        normaAttack = StartScript.GetStartScript.staminaController.CanNormalAttack;
        heavyAttack = StartScript.GetStartScript.staminaController.CanHeavyAttack;
    }
}
