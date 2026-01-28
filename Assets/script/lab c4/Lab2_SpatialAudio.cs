using UnityEngine;

public class Lab2_SpatialAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public bool is3D = true; // Toggle giữa 2D và 3D

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateSpatialBlend();
    }

    void Update()
    {
        // Nhấn T để toggle 2D/3D
        if (Input.GetKeyDown(KeyCode.T))
        {
            is3D = !is3D;
            UpdateSpatialBlend();
        }
    }

    void UpdateSpatialBlend()
    {
        if (is3D)
        {
            audioSource.spatialBlend = 1.0f; // 3D
            Debug.Log("Audio Mode: 3D (Spatial)");
        }
        else
        {
            audioSource.spatialBlend = 0.0f; // 2D
            Debug.Log("Audio Mode: 2D (Non-spatial)");
        }
    }
}