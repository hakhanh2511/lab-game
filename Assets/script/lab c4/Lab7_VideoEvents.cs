using UnityEngine;
using UnityEngine.Video;

public class Lab7_VideoEvents : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found!");
            return;
        }

        // Đăng ký event khi video kết thúc
        videoPlayer.loopPointReached += OnVideoFinished;
        
        // Tự động phát video khi bắt đầu
        videoPlayer.Play();
        Debug.Log("Video Started Automatically");
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Tự động restart khi video kết thúc
        Debug.Log("Video Finished - Restarting...");
        vp.Stop();
        vp.Play();
    }

    void Update()
    {
        // Giữ lại phím V để Play/Pause thủ công nếu cần
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

        // Giữ lại phím R để Restart thủ công nếu cần
        if (Input.GetKeyDown(KeyCode.R))
        {
            videoPlayer.Stop();
            videoPlayer.Play();
            Debug.Log("Video Restarted Manually");
        }
    }

    void OnDestroy()
    {
        // Hủy đăng ký event khi object bị destroy
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}