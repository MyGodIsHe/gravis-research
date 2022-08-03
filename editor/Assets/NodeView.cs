using UnityEngine;

public class NodeView : MonoBehaviour
{
    public Node nodeLink;
    
    [SerializeField] private Transform origin;
    [SerializeField] private TextMesh text;

    public void SetText(string value)
    {
        text.text = value;
    }

    private void Update()
    {
        origin.up = Camera.main.transform.up;
        origin.forward = Camera.main.transform.forward * -1f;
    }
}
