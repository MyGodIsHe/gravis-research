using Nodes.Enums;

namespace UI.Selection
{
    public class NodeForceWheelSelector : TypedWheelSelectorBase<ENodeForce>
    {
        protected override string GetKey(ENodeForce value)
        {
            return value.ToString();
        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public static NodeForceWheelSelector Instance
        {
            get;
            private set;
        }
    }
}