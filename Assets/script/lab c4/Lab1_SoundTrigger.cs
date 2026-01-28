using UnityEngine;

public class Lab4_SoundTrigger : MonoBehaviour
{
   public AudioSource audioSource;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
            Debug.Log("Audio Played");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            audioSource.Stop();
            Debug.Log("Audio Stopped");
        }
    }
}