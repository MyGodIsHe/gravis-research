using Nodes.Enums;
using UnityEngine;

namespace UI.Selection
{
    public class NodeForceWheelSelectorDisplay : WheelSelectorDisplayBase<ENodeForce>
    {
        [SerializeField] private WheelSelectorDisplayBase<string> display;
        
        public override void Display(ENodeForce value)
        {
            display.Display(value.ToString());
        }

        public override void SetColor(Color color)
        {
            display.SetColor(color);
        }
    }
}