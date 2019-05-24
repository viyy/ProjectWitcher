using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    //Сериализованное поле для ссылки на игрока.
    [SerializeField] private GameObject Player;

    [SerializeField] private float CameraMinDistance = 5.0f;

    [SerializeField] private float CameraMaxDistance = 15f;

    [SerializeField] private float CameraZoomSpeed = 300f;

    [SerializeField] private float AxisX_MouseSensivity = 3f;

    [SerializeField] private float AxisY_MouseSensivity = 3f;

    //Угол вращения камеры по оси Y.
    private float RotationY;
    //Угол вращения камеры по оси X.
    private float RotationX;
    //Вектор расстояния между игроком и камерой.
    private Vector3 Offset;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //Присваимем переменной угол по оси y;
        RotationY = transform.eulerAngles.y;

        //Вычисляем расстояние между камерой и игроком.
        Offset = Player.transform.position - transform.position;
    }

    void LateUpdate()
    {
        float Zoom = Input.GetAxis("Mouse ScrollWheel");

        //Управление зумом камеры.
        if (Zoom != 0)
        {
            Offset.z -= (1 * Input.GetAxis("Mouse ScrollWheel")) * CameraZoomSpeed * Time.deltaTime;

            //Ограничиваем приближение\отдаление камеры.
            Offset.z = Mathf.Clamp(Offset.z, CameraMinDistance, CameraMaxDistance);
        }

        RotationY += Input.GetAxis("Mouse X") * (AxisX_MouseSensivity * 2);
        RotationX += -Input.GetAxis("Mouse Y") * (AxisY_MouseSensivity * 2);

        //Ограничиваем движение камеры
        RotationX = Mathf.Clamp(RotationX, 0, 70);

        //Преобразуем угол Еулера по оси Y в кватернион.
        Quaternion rotation = Quaternion.Euler(RotationX, RotationY, 0);

        //Задаем позицию камеры как Vector3 игрока минус оффсет умноженное угол вращения.
        transform.position = Player.transform.position - (rotation * Offset);

        //Камера все время повернута в сторону игрока.
        transform.LookAt(Player.transform);
    }
}
