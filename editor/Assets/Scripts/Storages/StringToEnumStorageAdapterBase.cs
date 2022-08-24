using Storages.Interfaces;

namespace Storages
{
    public abstract class StringToEnumStorageAdapterBase<TEnum, TValue> : IStorage<TEnum, TValue>
    {
        public StringToEnumStorageAdapterBase(IStorage<string, TValue> storage)
        {
            _storage = storage;
        }

        private readonly IStorage<string, TValue> _storage;

        public TValue GetValue(TEnum key)
        {
            var stringKey = key.ToString();
            
            var value = _storage.GetValue(stringKey);
            return value;
        }
    }
}