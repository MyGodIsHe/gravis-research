using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    public GameObject blankButton;
    public GameObject scrollContent;

    public void Activate()
    {
        gameObject.SetActive(true);
        string[] filePaths = Directory.GetFiles(@"../examples/", "*.g");
        foreach (var filePath in filePaths)
        {
            var gameObject = Instantiate(blankButton, scrollContent.transform);
            var text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            text.text = Path.GetFileNameWithoutExtension(filePath);
            var button = gameObject.GetComponentInChildren<Button>();
            button.onClick.AddListener(Deactivate);
            var loader = gameObject.GetComponent<Loader>();
            loader.filePath = filePath;
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        foreach (Transform child in scrollContent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
