#nullable enable
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using OpenVRUtil;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DashboardOverlay : MonoBehaviour
{
    public RenderTexture renderTexture;
    public GraphicRaycaster GraphicRaycaster;
    public EventSystem EventSystem;
    
    private ulong dashboardHandle = OpenVR.k_ulOverlayHandleInvalid;
    private ulong thumbnailHandle = OpenVR.k_ulOverlayHandleInvalid;
    
    void Start()
    {
        OpenVRUtil.System.InitOpenVR();
        
        (dashboardHandle, thumbnailHandle) = Overlay.CreateDashboardOverlay("DiscordDashboardKey", "Discord Overlay Setting");
        
        var filePath = Application.streamingAssetsPath + "/icon.png";
        Overlay.SetOverlayFromFile(thumbnailHandle, filePath);
        
        Overlay.FlipOverlayVertical(dashboardHandle);
        Overlay.SetOverlaySize(dashboardHandle, 2.5f);

        Overlay.SetOverlayMouseScale(dashboardHandle, renderTexture.width, renderTexture.height);
    }

    private void Update()
    {
        Overlay.SetOverlayRenderTexture(dashboardHandle, renderTexture);

        ProcessOverlayEvents();
    }

    private void OnApplicationQuit()
    {
        Overlay.DestroyOverlay(dashboardHandle);
    }

    private void OnDestroy()
    {
        OpenVRUtil.System.ShutdownOpenVR();
    }

    private T? GetComponentByPosition<T>(Vector2 position)
    {
        var pointerEventData = new PointerEventData(EventSystem)
        {
            position = position
        };
        var results = new List<RaycastResult>();
        GraphicRaycaster.Raycast(pointerEventData, results);
        foreach (var result in results)
        {
            var component = result.gameObject.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
        }

        return default;
    }

    private void ProcessOverlayEvents()
    {
        var vrEvent = new VREvent_t();
        var uncbVREvent = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(VREvent_t));

        while (OpenVR.Overlay.PollNextOverlayEvent(dashboardHandle, ref vrEvent, uncbVREvent))
        {
            switch ((EVREventType)vrEvent.eventType)
            {
                case EVREventType.VREvent_MouseButtonDown:
                    vrEvent.data.mouse.y = renderTexture.height - vrEvent.data.mouse.y;
                    var button = GetComponentByPosition<CustomButton>(new Vector2(vrEvent.data.mouse.x, vrEvent.data.mouse.y));
                    if (button != null)
                    {
                        button.OnClick.Invoke();
                    }
                    break;
            }
        }
    }
}
