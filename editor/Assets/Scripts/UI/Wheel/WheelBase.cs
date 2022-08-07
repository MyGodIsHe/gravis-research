using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public abstract class WheelBase<T> : MonoBehaviour
    {
        [SerializeField] private float min;
        [SerializeField] private float max;

        [SerializeField] private GameObject view;
        [SerializeField] private Transform root;
        [SerializeField] private ElementBase<T> prefab;

        private List<ElementContainer> _elements = new List<ElementContainer>();

        protected abstract Action Hook(T value, ElementBase<T> element);

        public void Init(IEnumerable<T> texts)
        {
            var list = texts.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var value = list[i];

                var delta = max - min;
                var element = Instantiate(prefab, root);
                var amount = min + delta / list.Count / 360f;
                var rotation = amount * i * 360f;
                
                element.SetValue(value);
                element.SetAmount(amount);
                element.SetRotation(rotation);

                var action = Hook(value, element);

                var container = new ElementContainer()
                {
                    element = element,
                    min = rotation,
                    max = rotation + amount * 360f,
                    action = action
                };
                
                _elements.Add(container);
            }
        }

        public void Show(Vector3 position)
        {
            transform.position = position;
            view.SetActive(true);
        }

        public void Hide()
        {
            view.SetActive(false);
        }

        private void Update()
        {
            if (!view.activeSelf) return;
             
            var delta = Input.mousePosition - transform.position;
            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg + 180f;
            var target = _elements.OrderBy(value => Mathf.Abs(angle - Mathf.Lerp(value.min, value.max, 0.5f) - (value.max - value.min))).First();

            if (Input.GetMouseButtonDown(0))
            {
                target.action.Invoke();
            }
        }

        private struct ElementContainer
        {
            public ElementBase<T> element;
            
            public float min;
            public float max;
            
            public Action action;
        }
    }
}