using System.Threading.Tasks;

namespace UI.Selection
{
    public abstract class NodeTextStrategyBase
    {
        public abstract Task<string> GetText();
    }
}