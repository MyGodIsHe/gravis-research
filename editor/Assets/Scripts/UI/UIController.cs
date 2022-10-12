using Inputs;
using Settings;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIController : IInitializable, ILateDisposable
    {
        public UIController(UISettings uiSettings, InputService inputService, GameObject menu)
        {
            _uiSettings = uiSettings;
            _inputService = inputService;
            _menu = menu;
        }

        private readonly UISettings _uiSettings;
        private readonly InputService _inputService;
        private readonly GameObject _menu;

        private bool _canToggleMenu = false;
        
        public void Initialize()
        {
            _inputService.Register(_uiSettings.ToggleMenuKey, ToggleMenu);
        }

        public void LateDispose()
        {
            _inputService.Unregister(_uiSettings.ToggleMenuKey);
        }

        public void SetToggleMenuPermission(bool can)
        {
            _canToggleMenu = can;
        }

        private void ToggleMenu()
        {
            if (!_canToggleMenu) return;
            
            _menu.SetActive(!_menu.activeSelf);
        }
    }
}