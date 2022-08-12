using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Selection
{
    public class NodeTypeWheelSelector : WheelSelectorBase<NodeType>
    {
        [SerializeField] private float radius;
        [SerializeField] private WheelSelectorItem prefab;
        [SerializeField] private SpriteNodeTypeDictionary items;

        protected override IDictionary<WheelSelectorItem, NodeType> GenerateItems(GameObject root)
        {
            var generatedItems = new Dictionary<WheelSelectorItem, NodeType>();

            int i = 0;
            foreach (var (item, type) in items)
            {
                var amount = (float) i / items.Count * 360f * Mathf.Deg2Rad;
                var position = new Vector2(Mathf.Cos(amount), Mathf.Sin(amount)) * radius;
                
                var generatedItem = Instantiate(prefab, root.transform);
                generatedItem.SetSprite(item);
                
                generatedItem.transform.localPosition = position;
                generatedItem.Bisector = position;
                generatedItem.Width = 360f / items.Count;
                
                generatedItems.Add(generatedItem, type);
                
                i++;
            }

            return generatedItems;
        }

        protected override WheelSelectorItem GetCurrentItem(IEnumerable<WheelSelectorItem> items)
        {
            var point = (Vector2) (Input.mousePosition - transform.position);
            
            var closest = items.OrderBy(value => (value.Bisector - point).magnitude).First();
            return closest;
        }

        protected override bool IsSelectRequested()
        {
            return Input.GetMouseButtonUp(0);
        }

        protected override void Start()
        {
            base.Start();
            
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public static NodeTypeWheelSelector Instance
        {
            get;
            private set;
        }
        
        [Serializable]
        public class SpriteNodeTypeDictionary : SerializableDictionary<Sprite, NodeType> {}
    }
}