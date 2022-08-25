using System.Collections.Generic;
using System.Linq;
using Storages.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.Selection
{
    public abstract class TypedWheelSelectorBase<T> : WheelSelectorBase<T>
    {
        [SerializeField] private float radius;
        [SerializeField] private float size;
        
        [SerializeField] private WheelSelectorItemBase<Sprite> prefab;
        [SerializeField] private List<T> values;

        private IStorage<string, Sprite> _storage;

        [Inject]
        private void Construct(IStorage<string, Sprite> storage)
        {
            _storage = storage;
        }

        protected abstract string GetKey(T value);

        protected override IDictionary<WheelSelectorItemBase, T> GenerateItems(GameObject root)
        {
            var generatedItems = new Dictionary<WheelSelectorItemBase, T>();

            int i = 0;
            foreach (var value in values)
            {
                var amount = (float) i / values.Count * 360f * Mathf.Deg2Rad;
                var position = new Vector2(Mathf.Cos(amount), Mathf.Sin(amount)) * radius;
                var size = Vector2.one * this.size;
                
                var generatedItem = Instantiate(prefab, root.transform);
                var key = GetKey(value);
                var sprite = _storage.GetValue(key);
                generatedItem.SetValue(sprite);

                var rectTransform = (RectTransform) generatedItem.transform;
                rectTransform.localPosition = position;
                rectTransform.sizeDelta = size;
                generatedItem.Bisector = position;
                generatedItem.Width = 360f / values.Count;
                
                generatedItems.Add(generatedItem, value);
                
                i++;
            }

            return generatedItems;
        }

        protected override WheelSelectorItemBase GetCurrentItem(IEnumerable<WheelSelectorItemBase> items)
        {
            var point = (Vector2) (Input.mousePosition - transform.position);
            
            var closest = items.OrderBy(value => (value.Bisector - point).magnitude).First();
            return closest;
        }

        protected override bool IsSelectRequested()
        {
            return Input.GetMouseButtonUp(0);
        }
    }
}