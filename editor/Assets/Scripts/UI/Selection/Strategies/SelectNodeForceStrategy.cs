using System.Threading.Tasks;
using Nodes.Enums;

namespace UI.Selection.Strategies
{
    public class SelectNodeForceStrategy : NodeForceStrategyBase
    {
        public override async Task<ENodeForce> SelectForce()
        {
            var wheel = NodeForceWheelSelector.Instance;

            var result = await wheel.Select();
            return result;
        }
    }
}