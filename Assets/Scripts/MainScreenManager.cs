using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Configurator configurator;

    private void Start()
    {
        SetText();
    }

    public void SetText()
    {
        string text = "DiscOverlay Info \n" +
                      "\n" +
                      "File Path: " + Application.persistentDataPath + "/settings.json\n" +
                      "Position: \n" +
                      "X: " + configurator.settingsAsset.x + "\tY: " + configurator.settingsAsset.y + "\tZ: " +
                      configurator.settingsAsset.z + "\n" +
                      "Rotation: \n" +
                      "X: " + configurator.settingsAsset.rotationX + "\tY: " + configurator.settingsAsset.rotationY +
                      "\tZ: " + configurator.settingsAsset.rotationZ + "\n" +
                      "Size: " + configurator.settingsAsset.size + "\n" +
                      "Server ID: " + configurator.settingsAsset.serverId + "\n" +
                      "Channel ID: " + configurator.settingsAsset.channelId + "\n" +
                      "Custom URL: " + configurator.settingsAsset.customUrl;
        infoText.SetText(text);
    }

    public void OpenFile()
    {
        Application.OpenURL("file://" + Application.persistentDataPath);
    }
}