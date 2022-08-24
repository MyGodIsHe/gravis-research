using UnityEngine;

namespace UI.Selection
{
    public abstract class WheelSelectorItemBase<T> : WheelSelectorItemBase
    {
        public abstract void SetValue(T value);
    }
    
    public abstract class WheelSelectorItemBase : MonoBehaviour
    {
        public abstract void SetColor(Color color);
        
        public Vector2 Bisector
        {
            get;
            set;
        }

        public float Width
        {
            get;
            set;
        }
    }
}