using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class miniproject_VideoPlayerBasic : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    
    [Header("UI References")]
    [SerializeField] private Button skipButton;

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
        
        // Đăng ký sự kiện cho nút Skip
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipVideo);
        }
        else
        {
            Debug.LogWarning("Skip Button not assigned!");
        }
        
        // Tự động phát video khi bắt đầu
        videoPlayer.Play();
        Debug.Log("Video Started Automatically");
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Khi video kết thúc tự nhiên, chuyển sang game
        Debug.Log("Video Finished - Starting Game...");
        StartGame();
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

        // Vẫn giữ phím Space để skip nhanh
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipVideo();
        }
    }

    void SkipVideo()
    {
        Debug.Log("Video Skipped by user!");
        videoPlayer.Stop();
        
        // Ẩn nút skip sau khi bấm
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
        }
        
        // Chuyển sang game
        StartGame();
    }

    void StartGame()
    {
        // Ẩn canvas video
        if (skipButton != null)
        {
            skipButton.transform.parent.gameObject.SetActive(false); // Ẩn cả Canvas
        }
        
        // Chuyển sang scene game
        // SceneManager.LoadScene("GameScene");
        // hoặc
        // GameManager.Instance.StartGame();
        
        Debug.Log("Game Started!");
    }

    void OnDestroy()
    {
        // Hủy đăng ký event
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
        
        if (skipButton != null)
        {
            skipButton.onClick.RemoveListener(SkipVideo);
        }
    }
}