using System;
using UnityEngine;
using VoltstroStudios.UnityWebBrowser;
using VoltstroStudios.UnityWebBrowser.Core;

public class BrowserManager : MonoBehaviour
{
    [SerializeField] private BaseUwbClientManager uwbClientManager;
    private WebBrowserClient _webBrowserClient;

    private void OnEnable()
    {
        _webBrowserClient = uwbClientManager.browserClient;
    }

    public void LoadUrl(string url)
    {
        try
        {
            _webBrowserClient.LoadUrl(url);
            // webBrowserClient.Refresh();
        }
        catch (UwbIsNotReadyException)
        {
            _webBrowserClient.initialUrl = url;
            // webBrowserClient.OnClientInitialized += webBrowserClient.Refresh;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void LoadUrlFromId(string guildId, string channelId)
    {
        string url =
            $"https://streamkit.discord.com/overlay/voice/{guildId}/{channelId}?icon=true&online=true&logo=white&text_color=%23ffffff&text_size=14&text_outline_color=%23000000&text_outline_size=0&text_shadow_color=%23000000&text_shadow_size=0&bg_color=%231e2124&bg_opacity=0.95&bg_shadow_color=%23000000&bg_shadow_size=0&invite_code=&limit_speaking=false&small_avatars=false&hide_names=false&fade_chat=0&streamer_avatar_first=false";
        try
        {
            _webBrowserClient.LoadUrl(url);
            // webBrowserClient.Refresh();
        }
        catch (UwbIsNotReadyException)
        {
            _webBrowserClient.initialUrl = url;
            // webBrowserClient.OnClientInitialized += webBrowserClient.Refresh;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void Reload()
    {
        try
        {
            _webBrowserClient.Refresh();
        }
        catch (UwbIsNotReadyException)
        {
            _webBrowserClient.OnClientInitialized += _webBrowserClient.Refresh;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}