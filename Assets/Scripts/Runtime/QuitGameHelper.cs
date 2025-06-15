using UnityEngine;

namespace Assets.Scripts.Runtime {
    sealed class QuitGameHelper : MonoBehaviour {
#if UNITY_WEBPLAYER
    public static string webplayerQuitURL = "https://gregorsoenn.itch.io/press-for-peace";
#endif
        public static void Quit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
        }
    }
}