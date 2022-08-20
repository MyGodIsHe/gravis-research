using System;
using DG.Tweening;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class NavigationSettings
    {
        [Tooltip("Клавиша, при нажатии на которую, будет произведено перемещение к следующей ноде")]
        [field: SerializeField]
        public KeyCode MoveKey
        {
            get;
            private set;
        }
        
        [Tooltip("Продолжительность перемещения камеры к следующему узлу")]
        [field: SerializeField]
        public float MoveDuration
        {
            get;
            private set;
        }
        
        [Tooltip("Параметр для нелинейного перемещения камеры")]
        [field: SerializeField]
        public Ease MoveEase
        {
            get;
            private set;
        }
    }
}