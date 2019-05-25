using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MovementController
{
    private Vector3 moveDirection = Vector3.zero;

    void Update()
    {
        if (_controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y += gravity * Time.deltaTime;

        // Move the controller
        _controller.Move(moveDirection * Time.deltaTime);
    }
}
