using UnityEngine;

public class Lab4_AudioTest : MonoBehaviour
{
    [Header("Assign Audio Clips")]
    public AudioClip bgmClip;
    public AudioClip sfxClip;
    public AudioClip voiceClip;

    void Start()
    {
        Debug.Log("========== LAB 4 - AUDIO INFO ==========");
        
        DisplayAudioInfo(bgmClip, "BGM");
        DisplayAudioInfo(sfxClip, "SFX");
        DisplayAudioInfo(voiceClip, "Voice");
    }

    void DisplayAudioInfo(AudioClip clip, string label)
    {
        if (clip == null)
        {
            Debug.LogWarning($"❌ {label}: No clip assigned!");
            return;
        }

        Debug.Log($"\n--- {label} Info ---");
        Debug.Log($"Name: {clip.name}");
        Debug.Log($"Length: {clip.length:F2} seconds");
        Debug.Log($"Frequency: {clip.frequency} Hz");
        Debug.Log($"Channels: {clip.channels} (1=Mono, 2=Stereo)");
        Debug.Log($"Load Type: {clip.loadType}");
        Debug.Log($"Load State: {clip.loadState}");
        Debug.Log($"Samples: {clip.samples}");
        
        // Tính dung lượng ước tính (uncompressed)
        float sizeInMB = (clip.samples * clip.channels * 2f) / (1024f * 1024f);
        Debug.Log($"Estimated Size (uncompressed): {sizeInMB:F2} MB");
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 600, 30), 
            "Lab 4 - Audio Import Settings Test", style);

        style.fontSize = 16;
        
        int y = 50;
        
        if (bgmClip != null)
        {
            GUI.Label(new Rect(10, y, 600, 25), 
                $"BGM: {bgmClip.name} | {bgmClip.length:F1}s | {bgmClip.loadType}", style);
            y += 30;
        }

        if (sfxClip != null)
        {
            GUI.Label(new Rect(10, y, 600, 25), 
                $"SFX: {sfxClip.name} | {sfxClip.length:F1}s | {sfxClip.loadType}", style);
            y += 30;
        }

        if (voiceClip != null)
        {
            GUI.Label(new Rect(10, y, 600, 25), 
                $"Voice: {voiceClip.name} | {voiceClip.length:F1}s | {voiceClip.loadType}", style);
        }
    }
}