using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    protected CharacterController _controller;
    
    //Камера игрока
    [SerializeField] private Transform Camera;

    [SerializeField] protected float jumpSpeed = 8.0f;

    [SerializeField] protected float speed = 5f;

    [SerializeField] private float runSpeed = 10f;
    
    [SerializeField] private float rotateSpeed = 15f;
    
    [SerializeField] private float mouseSensivity = 4f;
    
    [SerializeField] private KeyCode runButton = KeyCode.LeftShift;

    [SerializeField] private float walkStaminaDrain = 0.1f;

    [SerializeField] private float runStaminaDrain = 0.3f;

    [SerializeField] protected float gravity = -9.81f;
    
    private bool _isRunning = false;
    
    //Mock for unit data model
    private Unit unit = new Unit();


    //Вектор движения персонажа.
    private Vector3 Movement = Vector3.zero;

    void Start()
    {
       _controller =  gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {

        //Нулевой вектор.
        Movement = Vector3.zero;

        _isRunning = Input.GetKey(runButton) && unit.CanRun;
        
        var x = 0f;
   
        // check to see if the W or S key is being pressed.  
        var z = Input.GetAxis("Vertical") * (_isRunning ? runSpeed : speed);
   
        if (Math.Abs(z) > float.Epsilon)    
            // Mock for Stamina Drain
            unit.DrainStamina(_isRunning ? runStaminaDrain: walkStaminaDrain);
        // Move the character forwards or backwards
        _controller.SimpleMove(transform.forward * z); 
           
        // Check to see if the A or D key are being pressed
        x = Input.GetAxis("Horizontal");

   

        // Check to see if the right mouse button is pressed
        if (Input.GetMouseButton(1))
        {
            // Get the difference in horizontal mouse movement
            x = Input.GetAxis("Mouse X") * mouseSensivity;
        }


        //Если были нажаты клавиши то:
        if (z != 0 || x != 0)
        {
            //Добавляем скорость движения. Изменяем положение по осям x и z вектора3 + учитываем скорость.
            Movement.x = x;
            Movement.z = z;

            //Ограничиваем скорость движения по диагонали.
            Movement = Vector3.ClampMagnitude(Movement, runSpeed);

            //Кватернион для сохранения текущего вращения камеры.
            Quaternion TempCameraRotation = Camera.rotation;

            //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
            Camera.eulerAngles = new Vector3(0, Camera.eulerAngles.y, 0);

            //Переводим локальные координаты вектора движения игрока в глобальные.
            Movement = Camera.TransformDirection(Movement);

            //Возвращаем поворот камеры.
            Camera.rotation = TempCameraRotation;

            //Создаем кватернион направления движения, метод LookRotation() вычисляет кватернион который смотрит в направлении движения.
            Quaternion Direction = Quaternion.LookRotation(Movement);
            
            //Вращаем персонажа
            transform.rotation = Quaternion.Lerp(transform.rotation, Direction, rotateSpeed * Time.deltaTime);
        }   
        // rotate the character based on the x value
        //transform.Rotate(0, x*rotateSpeed*Time.deltaTime, 0);
        //Если на персонаж на поверхности, то имитируем силу тяжести.
        if (!_controller.isGrounded)
        {
            _controller.SimpleMove(_controller.velocity +  Vector3.down*gravity * Time.deltaTime);
        }

        //Двигаем персонажа
        //_controller.Move(Movement * Time.deltaTime);

    }
}
