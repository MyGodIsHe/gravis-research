using System.Threading.Tasks;

namespace UI.Selection.Strategies
{
    public abstract class NodeTextStrategyBase
    {
        public abstract Task<string> GetText();
    }
}