using UnityEngine;

public class Lab3_GlobalAudioControl : MonoBehaviour
{
    private bool isMuted = false;
    private bool isPaused = false;

    void Update()
    {
        // Phím M: Mute/Unmute
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }

        // Phím P: Pause/Resume
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        
        Debug.Log(isMuted ? "Audio: MUTED" : "Audio: UNMUTED");
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        AudioListener.pause = isPaused;
        
        Debug.Log(isPaused ? "Audio: PAUSED" : "Audio: RESUMED");
    }

    // Hiển thị UI để user biết trạng thái
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 300, 30), 
            "M: " + (isMuted ? "MUTED" : "UNMUTED"), style);
        GUI.Label(new Rect(10, 40, 300, 30), 
            "P: " + (isPaused ? "PAUSED" : "PLAYING"), style);
    }
}