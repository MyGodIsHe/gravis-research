namespace Storages.Interfaces
{
    public interface IStorage<in TKey, out TValue>
    {
        TValue GetValue(TKey key);
    }
}