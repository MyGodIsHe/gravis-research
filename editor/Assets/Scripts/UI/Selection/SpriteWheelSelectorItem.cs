using UnityEngine;
using UnityEngine.UI;

namespace UI.Selection
{
    public class SpriteWheelSelectorItem : WheelSelectorItemBase<Sprite>
    {
        [SerializeField] private Image image;
        
        public override void SetValue(Sprite value)
        {
            image.sprite = value;
        }

        public override void SetColor(Color color)
        {
            image.color = color;
        }
    }
}