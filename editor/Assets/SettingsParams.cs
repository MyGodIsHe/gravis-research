using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsParams : MonoBehaviour
{
    public Color nodeColor;
    public Color nodeColorOnSelected;
    public Color lineColor;
    public Color fontColor;
    public Color BgColor;

    public Image nodeColorImage;
    public Image nodeSelColorImage;
    public Image lineColorImage;
    public Image fontColorImage;
    public Image BgColorImage;

    public SettingsView settingsView;

    /*private void OnEnable() {
        nodeColorImage = settingsView.NodeColorButton.GetComponentInChildren<Image>();
        nodeSelColorImage = settingsView.CubeSelectColorButton.GetComponentInChildren<Image>();
        lineColorImage = settingsView.LineColorButton.GetComponentInChildren<Image>();
        fontColorImage = settingsView.FontColorButton.GetComponentInChildren<Image>();
        BgColorImage = settingsView.BackgroundColorButton.GetComponentInChildren<Image>();
    }*/

    void Update()
    {
        nodeColor = nodeColorImage.color;
        nodeColorOnSelected = nodeSelColorImage.color;
        lineColor = lineColorImage.color;
        fontColor = fontColorImage.color;
        BgColor = BgColorImage.color;
    }
}
