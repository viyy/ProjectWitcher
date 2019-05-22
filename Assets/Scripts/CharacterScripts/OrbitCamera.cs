using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    //Сериализованное поле для ссылки на игрока.
    [SerializeField] private Transform Player;

    [SerializeField] private float CameraMinDistance = 5.0f;

    [SerializeField] private float CameraMaxDistance = 15f;

    [SerializeField] private float CameraZoomSpeed = 300f;

    //Переменная для регулирования скорости вращения камеры.
    private float RotationSpeed = 3f;
    //Угол вращения камеры по оси Y.
    private float RotationY;
    //Угол вращения камеры по оси X.
    private float RotationX;
    //Вектор расстояния между игроком и камерой.
    private Vector3 Offset;
    
    void Start()
    {
        //Присваимем переменной угол по оси y;
        RotationY = transform.eulerAngles.y;
        
        //Вычисляем расстояние между камерой и игроком.
        Offset = Player.transform.position - transform.position;
    }
    
    void LateUpdate()
    {
        //Переменная в которую передаются данные о нажатых клавишах с горизонт стрелками(1\-1 = в зависимости от направления)
        float horInput = Input.GetAxis("Horizontal");

        float Zoom = Input.GetAxis("Mouse ScrollWheel");
        
        //Управление зумом камеры.
        if (Zoom != 0)
        {
            Offset.z -= (1 * Input.GetAxis("Mouse ScrollWheel")) * CameraZoomSpeed * Time.deltaTime;

            //Ограничиваем приближение\отдаление камеры.
            Offset.z = Mathf.Clamp(Offset.z, CameraMinDistance, CameraMaxDistance);
        }

        //Если клавиша была нажата:
        if (horInput != 0)
        {
            //Угол поворота камеры по оси Y равен направлению поворота (1\-1) умноженному на скорость вращения
            //Меняем угол поворота камеры.
            RotationY += horInput * RotationSpeed;
        }

        //Если клавиша не была нажата, то считываем движения по горизонтальной оси мыши.
        else
        {
            //Угол поворота камеры по оси Y равен направлению поворота (1\-1) умноженному на скорость вращения и на 3
            //Скорость вращения камеры мышью больше.
            RotationY += Input.GetAxis("Mouse X") * (RotationSpeed * 3);
            RotationX += -Input.GetAxis("Mouse Y") * (RotationSpeed * 3);


            //Ограничиваем движение камеры
            RotationX = Mathf.Clamp(RotationX, 0, 70);
        }

        //Преобразуем угол Еулера по оси Y в кватернион.
        Quaternion rotation = Quaternion.Euler(RotationX, RotationY, 0);

        //Задаем позицию камеры как Vector3 игрока минус оффсет умноженное угол вращения.
        transform.position = Player.position - (rotation * Offset);

        //Камера все время повернута в сторону игрока.
        transform.LookAt(Player);
    }
}
