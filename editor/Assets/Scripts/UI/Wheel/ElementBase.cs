using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class ElementBase<T> : MonoBehaviour
    {
        [SerializeField] private float contentRange;
        
        [SerializeField] private Image image;
        [SerializeField] private RectTransform content;

        public abstract void SetValue(T value);

        public void SetAmount(float value)
        {
            image.fillAmount = value;
        }

        public void SetRotation(float value)
        {
            var rotation = Quaternion.Euler(0f, 0f, value);
            image.transform.rotation = rotation;

            var amount = image.fillAmount * 360f;
            var angle = (value + amount + 12.5f) * Mathf.Deg2Rad;
            var position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * -contentRange;
            content.anchoredPosition = position;
        }
    }
}