using UnityEngine;

namespace UI.Selection
{
    public abstract class WheelSelectorDisplayBase<T> : MonoBehaviour
    {
        public abstract void Display(T value);
        public abstract void SetColor(Color color);
    }
}