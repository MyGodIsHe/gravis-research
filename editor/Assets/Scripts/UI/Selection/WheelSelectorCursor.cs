using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Selection
{
    public class WheelSelectorCursor : MonoBehaviour
    {
        public event Action<WheelSelectorItem> OnFollow = _ => { };

        [SerializeField] private float followDuration;
        
        [SerializeField] private Image image;

        private WheelSelectorItem _item;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _sequence;
        
        public void SetColor(Color color)
        {
            image.color = color;
        }

        public void SetItem(WheelSelectorItem item)
        {
            if (_item == item) return;

            _item = item;

            var targetRotation = Vector2.SignedAngle(Vector2.down, item.Bisector);
            var value = new Vector3(0f, 0f, targetRotation);
            
            _sequence?.Kill();
            _sequence = transform.DORotate(value, followDuration);
            _sequence.onComplete += Complete;

            void Complete()
            {
                OnFollow.Invoke(item);
            }
        }

        public void SetWidth(float width)
        {
            image.fillAmount = width;

            var rotation = Quaternion.Euler(0f, 0f, width * 360f / 2);
            image.transform.rotation = rotation;
        }
    }
}