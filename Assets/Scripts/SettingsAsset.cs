using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsAsset", menuName = "SettingsAsset", order = 1)]
[Serializable]
public class SettingsAsset : ScriptableObject
{
    public double x;
    public double y;
    public double z;
    public double rotationX;
    public double rotationY;
    public double rotationZ;
    public double size;
    public string serverId;
    public string channelId;
    public string customUrl;
    
    public static SettingsAsset CreateInstanceFromJson(string json)
    {
        SettingsAsset settingsAsset = CreateInstance<SettingsAsset>();
        JsonUtility.FromJsonOverwrite(json, settingsAsset);
        return settingsAsset;
    }
    
    public static SettingsAsset CreateBlankInstance()
    {
        SettingsAsset settingsAsset = CreateInstance<SettingsAsset>();
        settingsAsset.x = 0;
        settingsAsset.y = 0;
        settingsAsset.z = 1;
        settingsAsset.rotationX = 0;
        settingsAsset.rotationY = 0;
        settingsAsset.rotationZ = 0;
        settingsAsset.size = 1;
        settingsAsset.serverId = "";
        settingsAsset.channelId = "";
        settingsAsset.customUrl = "";
        return settingsAsset;
    }
}
