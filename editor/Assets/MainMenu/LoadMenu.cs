using System.Collections.Generic;
using System.IO;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadMenu : MonoBehaviour
{
    public GameObject blankButton;
    public GameObject scrollContent;

    private readonly IList<Button> _buttons = new List<Button>();

    private UIController _uiController;
    
    [Inject]
    private void Construct(UIController uiController)
    {
        _uiController = uiController;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _uiController.SetToggleMenuPermission(false);

        string[] filePaths = Directory.GetFiles(@"../examples/", "*.g");
        foreach (var filePath in filePaths)
        {
            var gameObject = Instantiate(blankButton, scrollContent.transform);
            
            var text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            text.text = Path.GetFileNameWithoutExtension(filePath);
            
            var button = gameObject.GetComponentInChildren<Button>();
            button.onClick.AddListener(Click);
            
            _buttons.Add(button);

            async void Click()
            {
                var gm = GraphManager.Get();
                gm.Clear();
                
                var loader = gameObject.GetComponent<Loader>();
                loader.filePath = filePath;
                await loader.Load();
                
                Deactivate();
            }
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        _uiController.SetToggleMenuPermission(true);

        foreach (var button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        if(gameObject.name != "SettingsMenu")
        {
            foreach (Transform child in scrollContent.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
