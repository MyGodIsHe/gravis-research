using System.Collections.Generic;
using System.Threading.Tasks;
using Settings;
using UI.Selection.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Selection
{
    public abstract class WheelSelectorBase<T> : MonoBehaviour, ISelector<T>
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private GameObject root;
        
        [SerializeField] private Image wheelImage;
        [SerializeField] private WheelSelectorCursor cursor;
        [SerializeField] private WheelSelectorDisplayBase<T> display;

        private IDictionary<WheelSelectorItem, T> _items = new Dictionary<WheelSelectorItem, T>();
        private T _currentValue;

        private ColorSettings _colorSettings;

        protected abstract IDictionary<WheelSelectorItem, T> GenerateItems(GameObject root);
        protected abstract WheelSelectorItem GetCurrentItem(IEnumerable<WheelSelectorItem> items);
        protected abstract bool IsSelectRequested();

        [Inject]
        private void Construct(ColorSettings colorSettings)
        {
            _colorSettings = colorSettings;
        }

        public void SetPosition(Vector2 position)
        {
            var size = rectTransform.sizeDelta;
            var rect = Camera.main.pixelRect;
            
            var x = Mathf.Clamp(position.x, size.x / 2, rect.xMax - size.x / 2);
            var y = Mathf.Clamp(position.y, size.y / 2, rect.yMax - size.y / 2);
            position = new Vector2(x, y);
            
            rectTransform.anchoredPosition = position;
        }
        
        public async Task<T> Select()
        {
            root.SetActive(true);
            
            while (!IsSelectRequested())
            {
                var item = GetCurrentItem(_items.Keys);
                cursor.SetItem(item);
                
                await Task.Yield();
            }

            root.SetActive(false);
            
            return _currentValue;
        }

        private void Follow(WheelSelectorItem item)
        {
            var value = _items[item];
            
            _currentValue = value;
            display.Display(value);
        }

        protected virtual void Start()
        {
            wheelImage.color = _colorSettings.MainWheelColor;

            _items = GenerateItems(root);
            foreach (var (item, _) in _items)
            {
                item.SetColor(_colorSettings.WheelItemColor);
            }
            
            cursor.SetColor(_colorSettings.WheelCursorColor);
            cursor.SetWidth(1f / _items.Count);

            display.SetColor(_colorSettings.DisplayTextColor);
        }

        private void OnEnable()
        {
            cursor.OnFollow += Follow;
        }

        private void OnDisable()
        {
            cursor.OnFollow -= Follow;
        }
    }
}