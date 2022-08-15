using UnityEngine;
using UnityEngine.UI;

namespace UI.Selection
{
    public class WheelSelectorItem : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }
        
        public void SetColor(Color color)
        {
            image.color = color;
        }

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