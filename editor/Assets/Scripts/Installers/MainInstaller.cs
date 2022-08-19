using Serialization;
using Zenject;

namespace Installers
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<SerializationService>()
                .AsSingle()
                .NonLazy();
        }
    }
}