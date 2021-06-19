using UnityEngine;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibration = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibration;
#endif

    public static void Vibrate(long milliseconds = 25) {
        if (isAndroid())
        {
            vibration.Call("vibrate", milliseconds);
        }
        else {
            Handheld.Vibrate();
        }
    }

    public static void Cancel() {
        if (isAndroid()) {
            vibration.Call("cancel");
        }
    }
    public static bool isAndroid() {
#if UNITY_ANDROID &&!UNITY_EDITOR
        return true;
#else
    return false;
#endif
    }
}
