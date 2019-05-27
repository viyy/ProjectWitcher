using UnityEngine;

public class Jump : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    
    [SerializeField] protected float jumpSpeed = 8.0f;

    protected CharacterController _controller;

    private void Awake()
    {
        _controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (_controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Move the controller
        _controller.Move(moveDirection * Time.deltaTime);
    }
}
