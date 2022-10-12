using System.Threading.Tasks;
using Nodes.Enums;

namespace UI.Selection.Strategies
{
    public class FixedNodeForceStrategy : NodeForceStrategyBase
    {
        public FixedNodeForceStrategy(ENodeForce value)
        {
            _value = value;
        }

        private readonly ENodeForce _value;

        public override Task<ENodeForce> SelectForce()
        {
            return Task.FromResult(_value);
        }
    }
}