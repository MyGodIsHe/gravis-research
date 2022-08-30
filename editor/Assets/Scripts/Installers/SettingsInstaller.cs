using Settings;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(menuName = "Installers/Settings", fileName = "SettingsInstaller")]
    public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        [SerializeField] private ColorSettings colorSettings;
        [SerializeField] private NavigationSettings navigationSettings;
        [SerializeField] private UISettings uiSettings;

        public override void InstallBindings()
        {
            Container
                .BindInstance(colorSettings)
                .AsSingle();

            Container
                .BindInstance(navigationSettings)
                .AsSingle();

            Container
                .BindInstance(uiSettings)
                .AsSingle();
        }
    }
}