using System.Threading.Tasks;

namespace UI.Selection.Strategies
{
    public class FixedNodeTextStrategy : NodeTextStrategyBase
    {
        public FixedNodeTextStrategy(string text)
        {
            _text = text;
        }

        private readonly string _text;
        
        public override Task<string> GetText()
        {
            return Task.FromResult(_text);
        }
    }
}