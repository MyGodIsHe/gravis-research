using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Constants;
using JetBrains.Annotations;
using SFB;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainGroup;
    [SerializeField] private GameObject performGroup;
    
    [SerializeField] private Button saveButton;

    [SerializeField] private Loader loader;

    private UIController _uiController;

    [Inject]
    private void Construct(UIController uiController)
    {
        _uiController = uiController;
    }

    private void OnEnable()
    {
        var gm = GraphManager.Get();
        
        var nodes = gm.GetNodes();
        var any = nodes != null && nodes.Any();
        
        saveButton.gameObject.SetActive(any);
    }

    public async void Save()
    {
        var directory = Application.persistentDataPath + SerializationConstants.SubfolderName;
        var name = SerializationConstants.DefaultName;
        var ex = new[]
        {
            SerializationHelper.Extension
        };

        var subfolder = Application.persistentDataPath + SerializationConstants.SubfolderName;
        if (!File.Exists(subfolder))
        {
            Directory.CreateDirectory(subfolder);
        }

        var path = StandaloneFileBrowser.SaveFilePanel($"Save graph to a file", directory, name, ex);

        var gm = GraphManager.Get();
        var nodes = gm.GetNodes().ToList();

        if (File.Exists(path))
        {
            File.Delete(path);    
        }

        if (path != string.Empty)
        {
            var stream = new StreamWriter(path, true);
            await using (stream)
            {
                var saver = new Saver(stream);
                await Task.Factory.StartNew(() => saver.Write(nodes));
            }
        }
    }

    [UsedImplicitly]
    public void Create()
    {
        mainGroup.SetActive(false);
        performGroup.SetActive(true);
    }

    [UsedImplicitly]
    public async void Load()
    {
        var ex = new[]
        {
            SerializationHelper.Extension
        };

        StandaloneFileBrowser.OpenFilePanelAsync($"Open a graph file", @$"../examples", ex, false, Select);

        async void Select(string[] paths)
        {
            var path = paths[0];
            
            var gm = GraphManager.Get();
            gm.Clear();
            
            loader.filePath = path;
            await loader.Load();

            gameObject.SetActive(false);
            _uiController.SetToggleMenuPermission(true);
        }
    }

    [UsedImplicitly]
    public async void Perform()
    {
        var gm = GraphManager.Get();
        gm.Clear();
        
        await loader.LoadEmpty();
        
        gameObject.SetActive(false);
        _uiController.SetToggleMenuPermission(true);

        Deny();
    }

    [UsedImplicitly]
    public void Deny()
    {
        mainGroup.SetActive(true);
        performGroup.SetActive(false);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
