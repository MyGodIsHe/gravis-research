using System;

namespace UI
{
    public class NodeWheel : WheelBase<NodeType>
    {
        private static NodeWheel SingleInstance;
        
        protected override Action Hook(NodeType value, ElementBase<NodeType> element)
        {
            return Listen;

            void Listen()
            {
                DragMouseOrbit.CreateNode(value, value.ToString());
                Hide();
            }
        }

        private void Awake()
        {
            if (SingleInstance != null && SingleInstance != this)
            {
                Destroy(this);
            }
            else
            {
                SingleInstance = this;
            }
        }

        private void Start()
        {
            var values = Enum.GetValues(typeof(NodeType)) as NodeType[];
            Init(values);
        }

        public static NodeWheel Instance => SingleInstance;
    }
}