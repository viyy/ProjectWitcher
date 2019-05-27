using UnityEngine;

namespace Assets.Scripts.BaseScripts
{
    //Базовый класс для всех активных объектов на сцене.
    public abstract class BaseObject : MonoBehaviour
    {
        private int _layer;
        private Transform _transform;

        public bool IsActive { get; private set; }

        //Строка для имени предмета.
        public string Name { get; private set; }

        //Переопределить в наследниках
        protected virtual void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        //Метод получает\задает слой объекта.
        public int Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
                LayerChange(gameObject.transform, _layer);
            }
        }

        //Метод для смены слоя объекта и всех вложенных объектов.
        private void LayerChange(Transform obj, int LayerNumber)
        {
            obj.gameObject.layer = LayerNumber;
            if (obj.childCount == 0) return;
            else
            {
                foreach (Transform child in obj)
                {
                    LayerChange(child, LayerNumber);
                }
            }
        }

        //Получаем компонент Transform
        public Transform GetTransform()
        {
            return _transform;
        }

        //Флаг активности.
        public void SetActive(bool state)
        {
            IsActive = state;
        }
    }
}
