using System.Threading.Tasks;

namespace UI.Selection.Interfaces
{
    public interface ISelector<T>
    {
        Task<T> Select();
    }
}