using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using VoltstroStudios.UnityWebBrowser.Core;

public class Configurator : MonoBehaviour
{
    [HideInInspector] public SettingsAsset settingsAsset;

    [Header("UI Elements")] [SerializeField]
    private ValueButton x;

    [SerializeField] private ValueButton y;
    [SerializeField] private ValueButton z;
    [SerializeField] private ValueButton rotationX;
    [SerializeField] private ValueButton rotationY;
    [SerializeField] private ValueButton rotationZ;
    [SerializeField] private ValueButton size;
    [SerializeField] private CustomButton loadButton;
    [SerializeField] private CustomButton saveButton;
    [SerializeField] private TextMeshProUGUI logText;

    [Header("Dependencies")] [SerializeField]
    private DiscordOverlay discordOverlay;

    [SerializeField] private BrowserManager browserManager;
    [SerializeField] private GameObject browserObject;
    [SerializeField] private MainScreenManager mainScreenManager;

    private void OnEnable()
    {
        LoadFromJsonAndAssign();
        SetBrowserUrl();
    }

    private void Start()
    {
        x.SetValue(settingsAsset.x);
        y.SetValue(settingsAsset.y);
        z.SetValue(settingsAsset.z);
        rotationX.SetValue(settingsAsset.rotationX);
        rotationY.SetValue(settingsAsset.rotationY);
        rotationZ.SetValue(settingsAsset.rotationZ);
        size.SetValue(settingsAsset.size);

        x.SetValueCallback += Apply;
        y.SetValueCallback += Apply;
        z.SetValueCallback += Apply;
        rotationX.SetValueCallback += Apply;
        rotationY.SetValueCallback += Apply;
        rotationZ.SetValueCallback += Apply;
        size.SetValueCallback += Apply;

        logText.SetText("");

        loadButton.OnClick += Load;
        saveButton.OnClick += Save;

        bool valid = ValidateIdAndUrl();
        browserObject.SetActive(valid);
    }

    public void Apply()
    {
        settingsAsset.x = x.value;
        settingsAsset.y = y.value;
        settingsAsset.z = z.value;
        settingsAsset.rotationX = rotationX.value;
        settingsAsset.rotationY = rotationY.value;
        settingsAsset.rotationZ = rotationZ.value;
        settingsAsset.size = size.value;
        
        discordOverlay.SetPositionAndSize();
    }

    public void Save()
    {
        Apply();
        string json = JsonUtility.ToJson(settingsAsset, true);
        Configurator.SaveToJson("settings.json", json);

        mainScreenManager.SetText();
        logText.SetText("Saved to " + GetFullPath("settings.json"));
    }

    private void SetBrowserUrl()
    {
        if (!string.IsNullOrEmpty(settingsAsset.customUrl))
        {
            browserManager.LoadUrl(settingsAsset.customUrl);
        }
        else
        {
            browserManager.LoadUrlFromId(settingsAsset.serverId, settingsAsset.channelId);
        }
    }

    private void LoadFromJsonAndAssign()
    {
        string json = Configurator.LoadFromJson("settings.json");
        SettingsAsset instanceFromJson = SettingsAsset.CreateInstanceFromJson(json);

        settingsAsset = instanceFromJson;
    }

    public void Load()
    {
        LoadFromJsonAndAssign();
        discordOverlay.SetPositionAndSize();

        logText.SetText("Loaded from settings.json");
        bool valid = ValidateIdAndUrl();
        browserObject.SetActive(valid);
        SetBrowserUrl();
        browserManager.Reload();
    }

    private static string GetFullPath(string dataPath)
    {
        return Application.persistentDataPath + "/" + dataPath;
    }

    private static bool SaveToJson(string dataPath, string json)
    {
        string fullPath = GetFullPath(dataPath);
        try
        {
            System.IO.File.WriteAllText(fullPath, json);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    private static string LoadFromJson(string dataPath)
    {
        string fullPath = GetFullPath(dataPath);
        try
        {
            if (!System.IO.File.Exists(fullPath))
            {
                SettingsAsset blankInstance = SettingsAsset.CreateBlankInstance();
                System.IO.File.WriteAllText(fullPath, JsonUtility.ToJson(blankInstance, true));
            }

            return System.IO.File.ReadAllText(fullPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    private bool ValidateIdAndUrl()
    {
        // serverId and channelId, or customUrl must be set
        // serverId and channelId must be 19 characters long, and all characters must be numbers
        // customUrl must be a valid URL
        // Debug.Log(settingsAsset);
        if (string.IsNullOrEmpty(settingsAsset.customUrl))
        {
            if (string.IsNullOrEmpty(settingsAsset.serverId) || string.IsNullOrEmpty(settingsAsset.channelId))
            {
                logText.SetText("serverId and channelId must be set, or customUrl must be set");
                return false;
            }

            if (settingsAsset.serverId.Length != 19 || settingsAsset.channelId.Length != 19)
            {
                logText.SetText("serverId and channelId must be 19 characters long");
                return false;
            }

            if (!long.TryParse(settingsAsset.serverId, out _) || !long.TryParse(settingsAsset.channelId, out _))
            {
                logText.SetText("serverId and channelId must be numbers");
                return false;
            }
        }
        else
        {
            if (!Uri.IsWellFormedUriString(settingsAsset.customUrl, UriKind.Absolute))
            {
                logText.SetText("customUrl must be a valid URL");
                return false;
            }
        }

        return true;
    }
}