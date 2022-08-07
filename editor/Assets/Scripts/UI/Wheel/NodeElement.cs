using TMPro;
using UnityEngine;

namespace UI
{
    public class NodeElement : ElementBase<NodeType>
    {
        [SerializeField] private TextMeshProUGUI text;
        
        public override void SetValue(NodeType value)
        {
            text.text = value.ToString();
        }
    }
}