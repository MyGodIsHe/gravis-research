using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
    public ChangeColor changeColor;

    public Toggle FogButton;
    public Toggle OrientationButton;
    public Dropdown ResolutionDropdown;
    public Button LineColorButton;
    public Button CubeSelectColorButton;
    public Button NodeColorButton;
    public Button FontColorButton;
    public Button BackgroundColorButton;
    public Button saveButton;
    public Resolution[] resolutions;

    public bool isFogOn = false;

    public GameObject buttonClicked;
    
    private void Start() {
        resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }
    
    void Update()
    {
        FogSwitch();
    }

    public void FogSwitch()
    {
        if(FogButton.isOn)
        {
            RenderSettings.fog = true;
        }
        else
        {
            RenderSettings.fog = false;
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void InitColor()
    {
        buttonClicked.GetComponent<ColorPreview>().previewGraphic.color = buttonClicked.GetComponent<ColorButton>().initColor;
    }
}
