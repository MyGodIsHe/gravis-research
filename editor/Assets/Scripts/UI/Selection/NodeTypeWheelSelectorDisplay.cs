using UnityEngine;

namespace UI.Selection
{
    public class NodeTypeWheelSelectorDisplay : WheelSelectorDisplayBase<NodeType>
    {
        [SerializeField] private WheelSelectorDisplayBase<string> display;
        
        public override void Display(NodeType value)
        {
            display.Display(value.ToString());
        }

        public override void SetColor(Color color)
        {
            display.SetColor(color);
        }
    }
}