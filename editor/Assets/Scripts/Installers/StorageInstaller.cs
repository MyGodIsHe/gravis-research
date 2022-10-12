using Storages;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(menuName = "Installers/Storage", fileName = "StorageInstaller")]
    public class StorageInstaller : ScriptableObjectInstaller<StorageInstaller>
    {
        [SerializeField] private ScriptableSpriteStorage spriteStorage;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<ScriptableSpriteStorage>()
                .FromInstance(spriteStorage)
                .AsSingle();

            Container
                .BindInterfacesTo<NodeForceSpriteStorageAdapter>()
                .AsSingle();

            Container
                .BindInterfacesTo<NodeTypeSpriteStorageAdapter>()
                .AsSingle();
        }
    }
}