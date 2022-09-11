using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class SettingsParams : MonoBehaviour
{
    private static SettingsParams settings;
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

    public static SettingsParams Get()
    {
        if (settings is null)
        {
            var gameObject = GameObject.Find("GRAPH_MANAGER");
            settings = gameObject.GetComponentInChildren<SettingsParams>();
        }
        return settings;
    }
    
    private void Awake() {
        if(settings == null)
        {
            settings = this;
        }

        if(File.Exists("Assets/settings.ini"))
        {
            //var ini = 
            print("ini");
        }
        //var set = 
        LoadSettings();

        
    }

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

    public void SaveSettings()
    {
        var settingsSave = new IniParser(@"Assets/Settings.ini");
        settingsSave.Write("Fog", settingsView.FogButton.isOn.ToString());
        settingsSave.Write("isFullscreen", settingsView.OrientationButton.isOn.ToString());
        settingsSave.Write("ColorSelectedNode", ColorToString(settingsView.CubeSelectColorButton.targetGraphic.color));
        settingsSave.Write("ColorNode", ColorToString(settingsView.NodeColorButton.targetGraphic.color));
        settingsSave.Write("ColorLine", ColorToString(settingsView.LineColorButton.targetGraphic.color));
        settingsSave.Write("ColorFont", ColorToString(settingsView.FontColorButton.targetGraphic.color));
        settingsSave.Write("ColorBg", ColorToString(settingsView.BackgroundColorButton.targetGraphic.color));
        settingsSave.Write("Resolution", settingsView.ResolutionDropdown.value.ToString());
    }

    public void LoadSettings()
    {   
        //var settingsSave = Resources.Load("Settings.ini");
        var settingsSave = new IniParser(@"Assets/Settings.ini");
        //string destination = "Assets/Settings.ini";
        //FileStream file;

        /*if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }*/

        //BinaryFormatter bf = new BinaryFormatter();
        //IniParser settingsSave = (IniParser) bf.Deserialize(file);
        /*var settingsSave : String;
        settingsSave = File.ReadAllText(destination);*/

        //StreamReader settingsSave = new StreamReader(destination);
        //Debug.Log(settingsSave.ReadToEnd());
        
        //file.Close();
        //Debug.Log(settingsSave.Read("Fog"));
        settingsView.FogButton.isOn = System.Convert.ToBoolean(settingsSave.Read("Fog"));
        settingsView.ResolutionDropdown.value = int.Parse(settingsSave.Read("Resolution"));
        settingsView.OrientationButton.isOn = System.Convert.ToBoolean(settingsSave.Read("isFullscreen"));
        settingsView.CubeSelectColorButton.targetGraphic.color = VectorFromString(settingsSave.Read("ColorSelectedNode"));
        settingsView.NodeColorButton.targetGraphic.color = VectorFromString(settingsSave.Read("ColorNode"));
        settingsView.LineColorButton.targetGraphic.color = VectorFromString(settingsSave.Read("ColorLine"));
        settingsView.FontColorButton.targetGraphic.color = VectorFromString(settingsSave.Read("ColorFont"));
        settingsView.BackgroundColorButton.targetGraphic.color = VectorFromString(settingsSave.Read("ColorBg"));       
    }

    private Vector4 VectorFromString(string savedColor)
    {
        if(savedColor.StartsWith("RGBA(") && savedColor.EndsWith(")"))
        {
            savedColor = savedColor.Substring(5, savedColor.Length-1);
            print(savedColor);
        }
        string[] sArray = savedColor.Split('/');
        Vector4 color = new Vector4(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]), float.Parse(sArray[3]));
        return color;
    }

    private string ColorToString(Color color)
    {
        float[] convertColor = {color.r, color.g, color.b, color.a};
        return new string(convertColor[0].ToString() + "/" + convertColor[1].ToString() + "/" + convertColor[2].ToString() + "/" + convertColor[3].ToString());
    }
}
