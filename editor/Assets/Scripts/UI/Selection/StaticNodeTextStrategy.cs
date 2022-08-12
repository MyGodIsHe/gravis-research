using System.Threading.Tasks;

namespace UI.Selection
{
    public class StaticNodeTextStrategy : NodeTextStrategyBase
    {
        public StaticNodeTextStrategy(string text)
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