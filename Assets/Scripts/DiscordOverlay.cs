using UnityEngine;
using Valve.VR;
using System;

public class DiscordOverlay : MonoBehaviour
{
    private ulong overlayHandle = OpenVR.k_ulOverlayHandleInvalid;
    
    private void Start()
    {
        InitOpenVR();

        overlayHandle = CreateOverlay("DiscordOverlayKey", "DiscordOverlay");
    }

    private void OnApplicationQuit()
    {
        DestroyOverlay(overlayHandle);
    }

    private void OnDestroy()
    {
        ShutdownOpenVR();
    }

    private void InitOpenVR()
    {
        if (OpenVR.System != null) return;
        
        var error = EVRInitError.None;
        OpenVR.Init(ref error, EVRApplicationType.VRApplication_Overlay);
        
        if (error != EVRInitError.None)
        {
            throw new Exception("Failed to initialize OpenVR: " + error);
        }
    }

    private ulong CreateOverlay(string key, string name)
    {
        var handle = OpenVR.k_ulOverlayHandleInvalid;
        var error = OpenVR.Overlay.CreateOverlay(key, name, ref handle);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to create overlay: " + error);
        }
        return handle;
    }
    
    private void ShutdownOpenVR()
    {
        if (OpenVR.System != null)
        {
            OpenVR.Shutdown();
        }
    }
    
    private void DestroyOverlay(ulong handle)
    {
        if (handle != OpenVR.k_ulOverlayHandleInvalid)
        {
            var error = OpenVR.Overlay.DestroyOverlay(handle);
            if (error != EVROverlayError.None)
            {
                throw new Exception("Failed to destroy overlay: " + error);
            }
        }
    }
}