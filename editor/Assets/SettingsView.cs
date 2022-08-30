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
    public Button LineColorButton;
    public Button CubeSelectColorButton;
    public Button NodeColorButton;
    public Button FontColorButton;
    public Button BackgroundColorButton;

    public bool isFogOn = false;

    public GameObject buttonClicked;


    private void OnEnable() {
        //FogButton.onValueChanged.AddListener(FogButton);
    }
    
    void Update()
    {
        FogSwitch();
        ScreenModeChange();
    }

    public void FogSwitch()
    {
        if(FogButton.isOn)
        {
            //Fog switch on
            Debug.Log("fog is on");
            RenderSettings.fog = true;
            //LightingSettings.Environment.fog = true;
        }
        else
        {
            //Fog switch off
            Debug.Log("fog is off");
            RenderSettings.fog = false;
        }
    }

    public void CubeColorOnSelected()
    {
        //changeColor.SelectColor
    }

    public void ScreenModeChange()
    {
        Screen.fullScreen = OrientationButton.isOn;
        if(OrientationButton.isOn)
        {
            //Screen.fullScreen;
        }

        else
        {
            //landscape orientation
        }
    }

    public void GetButtonClicked()
    {
        //buttonClicked = 
    }
}
