using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class UISettings
    {
        [Tooltip("Клавиша для открытия/закрытия меню")]
        [field: SerializeField]
        public KeyCode ToggleMenuKey
        {
            get;
            private set;
        }
    }
}