using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public ColorPicker colorPicker;
    public SettingsView settingsView;

    private void OnEnable() {
        GetComponent<Button>().onClick.AddListener(SetButtonClicked);
    }

    void Update()
    {
        //SetButtonClicked();
    }

    public void SwitchColorPicker()
    {
        colorPicker.gameObject.SetActive(!colorPicker.gameObject.activeSelf);
    }

    public void ActiveColorPicker()
    {
        colorPicker.gameObject.SetActive(true);
        colorPicker.FadeScreen.SetActive(true);
    }

    public void DisableColorPicker()
    {
        colorPicker.gameObject.SetActive(false);
        colorPicker.FadeScreen.SetActive(false);
    }

    public void SetButtonClicked()
    {
        settingsView.buttonClicked = gameObject;
    }

    
}
