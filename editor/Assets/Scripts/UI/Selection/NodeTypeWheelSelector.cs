namespace UI.Selection
{
    public class NodeTypeWheelSelector : TypedWheelSelectorBase<NodeType>
    {
        protected override string GetKey(NodeType value)
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
        
        public static NodeTypeWheelSelector Instance
        {
            get;
            private set;
        }
    }
}