using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif

using Agora.Rtc;
public class joinChannelVideo : MonoBehaviour
{
    // Fill in your app ID
    private string _appID = "a638e8503bf64005884d06e0d1d90bb0";
    // Fill in your channel name
    private string _channelName = "Main";
    // Fill in a temporary token
    private string _token = "007eJxTYODaIKX+pI6VV26PivBseyG/+wcnXdwbwvJCkdFi0eFJookKDIlmxhapFqYGxklpZiYGBqYWFiYpBmapBimGKZYGSUkGf1pK0xoCGRmOlO5kZWSAQBCfhcE3MTOPgQEAPtIcnA==";
    internal VideoSurface LocalView;
    internal VideoSurface RemoteView;
    internal IRtcEngine RtcEngine;

    void Start()
    {
        SetupVideoSDKEngine();
        InitEventHandler();
        SetupUI();
    }
    void Update()
    {
        CheckPermissions();
    }
    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            // Destroy IRtcEngine
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
#endif
    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    foreach (string permission in permissionList)
    {
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            Permission.RequestUserPermission(permission);
        }
    }
#endif
    }

    private void SetupUI()
    {
        GameObject go = GameObject.Find("LocalView");
        LocalView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);
        go = GameObject.Find("RemoteView");
        RemoteView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);
    }


    private void SetupVideoSDKEngine()
    {
        // Create an IRtcEngine instance
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext();
        context.appId = _appID;
        // Initialize the instance
        RtcEngine.Initialize(context);
    }
    // Create a user event handler instance and set it as the engine event handler
    private void InitEventHandler()
    {
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }
    public void Join()
    {
        // making the video visible
        LocalView.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
        RemoteView.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
        // Enable the video module
        RtcEngine.EnableVideo();
        // Set channel media options
        ChannelMediaOptions options = new ChannelMediaOptions();
        // Start video rendering
        LocalView.SetEnable(true);
        // Automatically subscribe to all audio streams
        options.autoSubscribeAudio.SetValue(true);
        // Automatically subscribe to all video streams
        options.autoSubscribeVideo.SetValue(true);
        // Set the channel profile to live broadcast
        options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
        //Set the user role as host
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Join a channel
        RtcEngine.JoinChannel(_token, _channelName, 0, options);
    }
    public void Leave()
    {
        // making the image(becomes plain white on triggerExit) transparent so that nothing can be seen instead of a white image
        LocalView.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
        RemoteView.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
        Debug.Log("Leaving _channelName");
        // Leave the channel
        RtcEngine.LeaveChannel();
        // Disable the video module
        RtcEngine.DisableVideo();
        // Stop remote video rendering
        RemoteView.SetEnable(false);
        // Stop local video rendering
        LocalView.SetEnable(false);
    }

    // Implement your own EventHandler class by inheriting the IRtcEngineEventHandler interface class implementation
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly joinChannelVideo _videoSample;
        internal UserEventHandler(joinChannelVideo videoSample)
        {
            _videoSample = videoSample;
        }
        // error callback
        public override void OnError(int err, string msg)
        {
        }
        // Triggered when a local user successfully joins the channel
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            _videoSample.LocalView.SetForUser(0, "");
        }
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            // Set the remote video display
            _videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            // Start video rendering
            _videoSample.RemoteView.SetEnable(true);
            Debug.Log("Remote user joined");
        }
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.RemoteView.SetEnable(false);
        }
    }
    

}
