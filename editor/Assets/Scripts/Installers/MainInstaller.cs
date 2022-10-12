using Inputs;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        [SerializeField] private GameObject menu;
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<InputService>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<UIController>()
                .AsSingle()
                .WithArguments(menu);
        }
    }
}