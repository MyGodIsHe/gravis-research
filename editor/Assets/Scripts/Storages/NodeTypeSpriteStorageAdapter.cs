using Storages.Interfaces;
using UnityEngine;

namespace Storages
{
    public class NodeTypeSpriteStorageAdapter : StringToEnumStorageAdapterBase<NodeType, Sprite>
    {
        public NodeTypeSpriteStorageAdapter(IStorage<string, Sprite> storage) 
            : base(storage)
        {
            
        }
    }
}