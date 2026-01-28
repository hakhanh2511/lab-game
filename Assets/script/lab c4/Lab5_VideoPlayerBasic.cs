using UnityEngine;
using UnityEngine.Video;

public class Lab5_VideoPlayerBasic : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found!");
        }
    }

    void Update()
    {
        // Nhấn V để Play/Pause
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                Debug.Log("Video Paused");
            }
            else
            {
                videoPlayer.Play();
                Debug.Log("Video Playing");
            }
        }

        // Nhấn R để Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            videoPlayer.Stop();
            videoPlayer.Play();
            Debug.Log("Video Restarted");
        }
    }
}