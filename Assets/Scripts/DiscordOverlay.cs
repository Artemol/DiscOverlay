using UnityEngine;
using Valve.VR;
using OpenVRUtil;

public class DiscordOverlay : MonoBehaviour
{
    public RenderTexture renderTexture;
    private ulong overlayHandle = OpenVR.k_ulOverlayHandleInvalid;

    [SerializeField] private Configurator configurator;

    private void Start()
    {
        OpenVRUtil.System.InitOpenVR();
        overlayHandle = Overlay.CreateOverlay("DiscordOverlayKey", "DiscordOverlay");

        Overlay.FlipOverlayVertical(overlayHandle);
        SetPositionAndSize(); 

        Overlay.ShowOverlay(overlayHandle);
    }


    private void Update()
    {
        Overlay.SetOverlayRenderTexture(overlayHandle, renderTexture);
    }

    public void SetPositionAndSize()
    {
        var position = new Vector3((float)configurator.settingsAsset.x, (float)configurator.settingsAsset.y, (float)configurator.settingsAsset.z);
        var rotation = Quaternion.Euler((float)configurator.settingsAsset.rotationX, (float)configurator.settingsAsset.rotationY,
            (float)configurator.settingsAsset.rotationZ);
        Overlay.SetOverlayTransformRelative(overlayHandle, OpenVR.k_unTrackedDeviceIndex_Hmd, position, rotation);
        Overlay.SetOverlaySize(overlayHandle, (float)configurator.settingsAsset.size);
    }

    private void OnApplicationQuit()
    {
        Overlay.DestroyOverlay(overlayHandle);
    }

    private void OnDestroy()
    {
        OpenVRUtil.System.ShutdownOpenVR();
    }
}