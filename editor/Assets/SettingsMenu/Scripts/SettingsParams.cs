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
    public INIParser iniParser;
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
        

        
    }
    private void Start() {
        LoadSettings();
    }

    public SettingsView settingsView;

    void Update()
    {
        nodeColor = nodeColorImage.color;
        nodeColorOnSelected = nodeSelColorImage.color;
        lineColor = lineColorImage.color;
        fontColor = fontColorImage.color;
        BgColor = BgColorImage.color;

        //settingsView.ResolutionDropdown.value = int.Parse(iniParser.ReadValue("GlobalSettings", "Resolution", "0"));
    }

    public void SaveSettings()
    {
        //var settingsSave = new IniParser(@"Assets/Settings.ini");
        iniParser = new INIParser();
        iniParser.Open(Application.persistentDataPath + "Settings.ini");
        iniParser.WriteValue("GlobalSettings", "Fog", settingsView.FogButton.isOn.ToString());
        iniParser.WriteValue("GlobalSettings", "isFullscreen", settingsView.OrientationButton.isOn.ToString());
        iniParser.WriteValue("GlobalSettings", "ColorSelectedNode", ColorToString(settingsView.CubeSelectColorButton.targetGraphic.color));
        iniParser.WriteValue("GlobalSettings", "ColorNode", ColorToString(settingsView.NodeColorButton.targetGraphic.color));
        iniParser.WriteValue("GlobalSettings", "ColorLine", ColorToString(settingsView.LineColorButton.targetGraphic.color));
        iniParser.WriteValue("GlobalSettings", "ColorFont", ColorToString(settingsView.FontColorButton.targetGraphic.color));
        iniParser.WriteValue("GlobalSettings", "ColorBg", ColorToString(settingsView.BackgroundColorButton.targetGraphic.color));
        iniParser.WriteValue("GlobalSettings", "Resolution", settingsView.ResolutionDropdown.value.ToString());
        iniParser.Close();
    }

    public void LoadSettings()
    {   
        iniParser = new INIParser();
        iniParser.Open(Application.persistentDataPath + "Settings.ini");
        //var settingsSave = Resources.Load("Settings.ini");
        //var settingsSave = new IniParser(@"Assets/Settings.ini");
        //var settingsSave = File.ReadAllText("Assets/Settings.ini") as TextAsset;
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
        settingsView.FogButton.isOn = System.Convert.ToBoolean(iniParser.ReadValue("GlobalSettings", "Fog", "true"));
        
        settingsView.OrientationButton.isOn = System.Convert.ToBoolean(iniParser.ReadValue("GlobalSettings", "isFullscreen", "true"));
        settingsView.CubeSelectColorButton.targetGraphic.color = VectorFromString(iniParser.ReadValue("GlobalSettings", "ColorSelectedNode","0/0/0/1"));
        settingsView.NodeColorButton.targetGraphic.color = VectorFromString(iniParser.ReadValue("GlobalSettings", "ColorNode", "1/1/1/1"));
        settingsView.LineColorButton.targetGraphic.color = VectorFromString(iniParser.ReadValue("GlobalSettings", "ColorLine", "1/0/0/1"));
        settingsView.FontColorButton.targetGraphic.color = VectorFromString(iniParser.ReadValue("GlobalSettings", "ColorFont", "1/1/0/1"));
        settingsView.BackgroundColorButton.targetGraphic.color = VectorFromString(iniParser.ReadValue("GlobalSettings", "ColorBg", "0/0/1/1"));       
        settingsView.ResolutionDropdown.value = int.Parse(iniParser.ReadValue("GlobalSettings", "Resolution", "0"));
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
