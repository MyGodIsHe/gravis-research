using Storages.Interfaces;
using UnityEngine;

namespace Storages
{
    public abstract class StorageBase<TKey, TValue> : ScriptableObject, IStorage<TKey, TValue>
    {
        public abstract TValue GetValue(TKey key);
    }
}