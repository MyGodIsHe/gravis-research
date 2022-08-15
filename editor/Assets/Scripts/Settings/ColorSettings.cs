using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class ColorSettings
    {
        [field: Header("Node Selection Wheel")]
        
        [Tooltip("Color of wheel's main pattern")]
        [field: SerializeField]
        public Color MainWheelColor
        {
            get;
            private set;
        }

        [Tooltip("Color of wheel's item icons")]
        [field: SerializeField]
        public Color WheelItemColor
        {
            get;
            private set;
        }

        [Tooltip("Color of wheel's selection cursor")]
        [field: SerializeField]
        public Color WheelCursorColor
        {
            get;
            private set;
        }

        [Tooltip("Color of display text")]
        [field: SerializeField]
        public Color DisplayTextColor
        {
            get;
            private set;
        }
    }
}