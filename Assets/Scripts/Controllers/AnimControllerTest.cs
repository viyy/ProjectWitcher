using UnityEngine;

public class AnimControllerTest : MonoBehaviour
{
    private Animator _animator;



    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _animator.SetFloat("horizontal", x);
        _animator.SetFloat("vertical", y);

        //transform.position += Vector3.forward * y * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _animator.SetBool("isRuning", true);
        }
        else
        {
            _animator.SetBool("isRuning", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool("isJumping", true);
        }
        else
        {
            _animator.SetBool("isJumping", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("isNormalAttack", true);
        }
        else
        {
            _animator.SetBool("isNormalAttack", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool("isHeavyAttack", true);
        }
        else
        {
            _animator.SetBool("isHeavyAttack", false);
        }
    }
}
