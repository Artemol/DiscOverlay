using UnityEngine;
using Valve.VR;
using System;

public class DiscordOverlay : MonoBehaviour
{
    public Camera camera;
    public RenderTexture renderTexture;
    private ulong overlayHandle = OpenVR.k_ulOverlayHandleInvalid;

    [SerializeField] private bool isDebuggingPosition = false;
    [Range(0, 3f)] public float size = 0.5f;
    [Range(-2f, 2f)] public float x;
    [Range(-2f, 2f)] public float y;
    [Range(-2f, 2f)] public float z;
    [Range(0, 360)] public float rotationX;
    [Range(0, 360)] public float rotationY;
    [Range(0, 360)] public float rotationZ;

    private void Start()
    {
        InitOpenVR();
        overlayHandle = CreateOverlay("DiscordOverlayKey", "DiscordOverlay");

        FlipOverlayVertical(overlayHandle);

        var position = new Vector3(x, y, z);
        var rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
        SetOverlayTransformRelative(overlayHandle, OpenVR.k_unTrackedDeviceIndex_Hmd, position, rotation);

        SetOverlaySize(overlayHandle, size);

        ShowOverlay(overlayHandle);
    }


    private void Update()
    {
        if (isDebuggingPosition)
        {
            var position = new Vector3(x, y, z);
            var rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
            SetOverlayTransformRelative(overlayHandle, OpenVR.k_unTrackedDeviceIndex_Hmd, position, rotation);
        }
        
        SetOverlayRenderTexture(overlayHandle, renderTexture);
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

    private void SetOverlayFromFile(ulong handle, string path)
    {
        var error = OpenVR.Overlay.SetOverlayFromFile(handle, path);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay from file: " + error);
        }
    }

    private void ShowOverlay(ulong handle)
    {
        var error = OpenVR.Overlay.ShowOverlay(handle);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to show overlay: " + error);
        }
    }

    private void SetOverlaySize(ulong handle, float size)
    {
        var error = OpenVR.Overlay.SetOverlayWidthInMeters(handle, size);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay width: " + error);
        }
    }

    private void SetOverlayTransformAbsolute(ulong handle, Vector3 position, Quaternion rotation)
    {
        var rigidTransform = new SteamVR_Utils.RigidTransform(position, rotation);
        var matrix = rigidTransform.ToHmdMatrix34();
        var error = OpenVR.Overlay.SetOverlayTransformAbsolute(handle,
            ETrackingUniverseOrigin.TrackingUniverseStanding, ref matrix);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay transform: " + error);
        }
    }

    private void SetOverlayTransformRelative(ulong handle, uint deviceIndex, Vector3 position, Quaternion rotation)
    {
        var rigidTransform = new SteamVR_Utils.RigidTransform(position, rotation);
        var matrix = rigidTransform.ToHmdMatrix34();
        var error = OpenVR.Overlay.SetOverlayTransformTrackedDeviceRelative(handle, deviceIndex, ref matrix);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay transform: " + error);
        }
    }

    private void FlipOverlayVertical(ulong handle)
    {
        var bounds = new VRTextureBounds_t
        {
            uMin = 0,
            uMax = 1,
            vMin = 1,
            vMax = 0
        };
        var error = OpenVR.Overlay.SetOverlayTextureBounds(handle, ref bounds);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay texture bounds: " + error);
        }
    }

    private void SetOverlayRenderTexture(ulong handle, RenderTexture renderTexture)
    {
        if (!renderTexture.IsCreated()) return;
        var nativeTexturePtr = renderTexture.GetNativeTexturePtr();

        var texture = new Texture_t
        {
            eType = ETextureType.DirectX,
            eColorSpace = EColorSpace.Auto,
            handle = nativeTexturePtr
        };

        var error = OpenVR.Overlay.SetOverlayTexture(handle, ref texture);
        if (error != EVROverlayError.None)
        {
            throw new Exception("Failed to set overlay texture: " + error);
        }
    }
}