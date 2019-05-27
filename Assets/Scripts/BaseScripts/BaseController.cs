using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.BaseScripts
{
    public abstract class BaseController
    {
        public bool _isActive { get; private set; }

        /// <summary>
        /// Включаем контроллер
        /// </summary>
        public virtual void On()
        {
            _isActive = true;
        }

        /// <summary>
        /// Выключаем контроллер
        /// </summary>
        public virtual void Off()
        {
            _isActive = false;
        }

        /// <summary>
        /// Переключаем контроллер
        /// </summary>
        public virtual void Switch()
        {
            _isActive = !_isActive;
        }

        public abstract void ControllerUpdate();

        ///Для переопределения в наследниках, если это необходимо
        public virtual void ControllerLateUpdate() { }
    }
}
