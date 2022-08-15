using Settings;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(menuName = "Installers/Settings", fileName = "SettingsInstaller")]
    public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        [SerializeField] private ColorSettings colorSettings;

        public override void InstallBindings()
        {
            Container
                .BindInstance(colorSettings)
                .AsSingle();
        }
    }
}