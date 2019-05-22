using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerFix : MonoBehaviour
{
    private CharacterController _controller;
    private StaminaUsing _staminaStatus;// для UI шкалы

    [SerializeField] private float speed = 5f;

    [SerializeField] private float runSpeed = 10f;

    [SerializeField] private float rotateSpeed = 60f;

    [SerializeField] private float mouseSensivity = 4f;
    
    [SerializeField] private KeyCode runButton = KeyCode.LeftShift;

    [SerializeField] private float walkStaminaDrain = 0.1f;

    [SerializeField] private float runStaminaDrain = 0.3f;

    [SerializeField] private float gravity = -9.81f;
    
    private bool _isRunning = false;
    
    //Mock for unit data model
    private Unit unit = new Unit();

    public bool RunCheck()       // проверка на бег для UI шкалы
    {
        if (_isRunning) return true;
        else return false;
    }
    
    void Start()
    {
       _controller =  gameObject.GetComponent<CharacterController>();
       _staminaStatus = GameObject.Find("StaminaScale").GetComponent<StaminaUsing>(); // для UI шкалы
    }

    void Update()
    {
        RunCheck();  // проверка на бег для UI шкалы

        _isRunning = Input.GetKey(runButton) && unit.CanRun && _staminaStatus.StaminaChekForRun(); //поправка для бега
        
        var x = 0f;
   
        // check to see if the W or S key is being pressed.  
        var z = Input.GetAxis("Vertical") * (_isRunning ? runSpeed : speed);
   
        
        if (Math.Abs(z) > float.Epsilon)    
            // Mock for Stamina Drain
            unit.DrainStamina(_isRunning ? runStaminaDrain: walkStaminaDrain);
        // Move the character forwards or backwards
        _controller.SimpleMove(transform.forward * z); 
           
        // Check to see if the A or S key are being pressed
        x = Input.GetAxis("Horizontal");

   
        // Check to see if the right mouse button is pressed
        if (Input.GetMouseButton(1))
        {
            // Get the difference in horizontal mouse movement
            x = Input.GetAxis("Mouse X") * mouseSensivity;
        }
 
        // rotate the character based on the x value
        transform.Rotate(0, x*rotateSpeed*Time.deltaTime, 0);

        if (!_controller.isGrounded)
        {
            _controller.SimpleMove(_controller.velocity + Vector3.down * gravity * Time.deltaTime);
        }
    }
}
