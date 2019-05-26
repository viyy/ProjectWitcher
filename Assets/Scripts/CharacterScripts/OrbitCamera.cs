using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    //Сериализованное поле для ссылки на игрока.
    [SerializeField] private GameObject Player;

    [SerializeField] private float CameraMinDistance = 4.0f;

    [SerializeField] private float CameraMaxDistance = 15f;

    [SerializeField] private float DistanceFromObstacle = 0.25f;

    [SerializeField] private float CameraZoomSpeed = 300f;

    [SerializeField] private float AxisX_MouseSensivity = 3f;

    [SerializeField] private float AxisY_MouseSensivity = 3f;

    [SerializeField] public float CameraObstacleAvoidSpeed = 5;

    [SerializeField] public float CameraReturnSpeed = 10;

    //Угол вращения камеры по оси Y.
    private float RotationY = 0;

    //Угол вращения камеры по оси X.
    private float RotationX = 0;

    //Приближение камеры.
    private float Zoom;

    //Вектор расстояния между игроком и камерой.
    private Vector3 Offset;

    //Расстояние до камеры с препятствием.
    private Vector3 ObstacleOffset;

    //Стартовое расстояние до камеры.
    private Vector3 StartCameraDistance;

    //Флаг для наличия препятствий
    private bool CameraObstacle;

    //Флаг автодвижения камеры.
    private bool AutoMove;

    //Луч для проверки препятствия перед камерой.
    private RaycastHit Ray;

    private Vector3 LastClearPosition;

    private Vector3 OffsetTemp;

    //Маска
    public LayerMask Mask;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //Вычисляем стартовое положение камеры.
        StartCameraDistance = Player.transform.position + (-Player.transform.forward * (CameraMinDistance + CameraMaxDistance / 2));

        //Задаем начальное расстояние.
        Offset = Player.transform.position - StartCameraDistance;

        //Запоминаем расстояние.
        OffsetTemp = Offset;
    }

    void LateUpdate()
    {
        //Получаем значения колесика мыши
        Zoom = Input.GetAxis("Mouse ScrollWheel");

        //Получаем значения движения мыши по осям X и Y
        RotationY += Input.GetAxis("Mouse X") * (AxisX_MouseSensivity * 2);
        RotationX += -Input.GetAxis("Mouse Y") * (AxisY_MouseSensivity * 2);

        //Ограничиваем движение камеры по оси X
        RotationX = Mathf.Clamp(RotationX, 0, 70);

        //Преобразуем угол Еулера в кватернион.
        Quaternion Rotation = Quaternion.Euler(RotationX, RotationY, 0);

        //Проверяем коллизию
        CollisionCheck(Player.transform.position, Rotation);

        //Двигаем камеру
        CameraMove(Rotation);

        //Камера все время повернута в сторону игрока.
        transform.LookAt(Player.transform);
    }

    /// Метод для передвижения камеры
    /// </summary>
    /// <param name="CameraObstacle">Флаг препятствия</param>
    /// <param name="Rotation">Текущее вращение камеры</param>
    private void CameraMove(Quaternion Rotation)
    {
        switch (CameraObstacle)
        {
            case true:

                transform.position = Vector3.Lerp(transform.position, ObstacleOffset, CameraObstacleAvoidSpeed * Time.deltaTime);
                break;

            case false:

                transform.position = Vector3.Lerp(transform.position, Player.transform.position - (Rotation * Offset), CameraReturnSpeed * Time.deltaTime);
                break;
        }

        //Управление зумом камеры.
        CameraZoom();
    }

    /// <summary>
    /// Метод проверки коллизий
    /// </summary>
    /// <param name="PlayerPosition">Позиция игрока</param>
    /// <param name="rotation">Вращение камеры</param>
    private void CollisionCheck(Vector3 PlayerPosition, Quaternion rotation)
    {
        Debug.DrawLine(PlayerPosition, (PlayerPosition - (rotation * Offset)), Color.yellow);

        //Проверяем столкновение луча с препятствием
        if (Physics.Linecast(PlayerPosition, (PlayerPosition - (rotation * Offset)), out Ray, Mask))
        {
            CameraObstacle = true;

            //Отдялаем новую позицию камеры от препятствия.
            ObstacleOffset = Ray.point + (rotation * (Ray.transform.forward * DistanceFromObstacle));

            Debug.DrawLine(Player.transform.position, ObstacleOffset, Color.red);

            return;
        }

        CameraObstacle = false;
    }

    /// <summary>
    /// Метод "зума" камеры
    /// </summary>
    /// <param name="MinDistance">Минимальная дистанция</param>
    /// <param name="MaxDistance">Максимальная дистанция</param>
    private void CameraZoom()
    {
        //Управление зумом камеры.
        if (Zoom != 0)
        {
            //Муняем зум на колесо мыши
            Offset.z -= (1 * Input.GetAxis("Mouse ScrollWheel")) * CameraZoomSpeed * Time.deltaTime;

            //Ограничиваем зум камеры.
            switch (CameraObstacle)
            {
                case true:
                    float MaxObstacleDist = Vector3.Distance(Player.transform.position, transform.position);
                    Offset.z = Mathf.Clamp(Offset.z, CameraMinDistance, MaxObstacleDist);
                    break;

                case false:
                    Offset.z = Mathf.Clamp(Offset.z, CameraMinDistance, CameraMaxDistance);
                    break;
            }
        }
    }
}
