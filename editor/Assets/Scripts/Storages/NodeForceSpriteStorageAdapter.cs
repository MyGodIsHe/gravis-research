using Nodes.Enums;
using Storages.Interfaces;
using UnityEngine;

namespace Storages
{
    public class NodeForceSpriteStorageAdapter : StringToEnumStorageAdapterBase<ENodeForce, Sprite>
    {
        public NodeForceSpriteStorageAdapter(IStorage<string, Sprite> storage) 
            : base(storage)
        {
            
        }
    }
}