
using UnityEngine;
using UnityEngine.UI;

public class ColorPreview : MonoBehaviour
{
    public Graphic previewGraphic;

    public ColourPicker colorPicker;

    private void Start()
    {
        colorPicker.onColorChanged += OnColorChanged;
        GetComponent<Button>().onClick.AddListener(GetPreview);
    }

    public void OnColorChanged(Color c)
    {   
        if(previewGraphic != null)
        previewGraphic.color = c;
    }

    public void GetPreview()
    {
        previewGraphic = transform.GetChild(0).GetComponent<Image>();
    }

    private void OnDestroy()
    {
        if (colorPicker != null)
            colorPicker.onColorChanged -= OnColorChanged;
    }
}