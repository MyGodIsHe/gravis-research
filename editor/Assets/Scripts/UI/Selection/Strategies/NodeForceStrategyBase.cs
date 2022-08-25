using System.Threading.Tasks;
using Nodes.Enums;

namespace UI.Selection.Strategies
{
    public abstract class NodeForceStrategyBase
    {
        public abstract Task<ENodeForce> SelectForce();
    }
}