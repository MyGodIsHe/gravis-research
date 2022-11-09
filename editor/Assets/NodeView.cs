using TMPro;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public Node nodeLink;
    public SettingsParams settingsParams;
    
    [SerializeField] private Transform origin;
    [SerializeField] private TextMeshPro text;

    public Transform Origin
    {
        get {return origin;}
    }

    public void SetText(string value)
    {
        text.text = value;
        text.color = settingsParams.fontColor;
    }

    private void Awake() {
        settingsParams = GameObject.Find("GRAPH_MANAGER").GetComponent<SettingsParams>();
    }

    private void Update()
    {
        var delta = Camera.main.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(delta, Camera.main.transform.up);

        origin.rotation = rotation;
    }
}
