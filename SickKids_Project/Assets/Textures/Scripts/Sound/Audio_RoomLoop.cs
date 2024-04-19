using UnityEngine;

public class Audio_RoomLoop : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public float crossfadeTime = 5f; // Duration of the crossfade
    private float timer = 0f;

    void Start()
    {
        // Start first audio source
        audioSource1.Play();
        audioSource1.volume = 1.0f;
        
        // Start second audio source
        audioSource2.Play();
        audioSource2.volume = 0.0f;
    }

    void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // Calculate volume levels based on timer
        if (timer < crossfadeTime)
        {
            audioSource1.volume = 1 - timer / crossfadeTime;
            audioSource2.volume = timer / crossfadeTime;
        }
        else
        {
            // Reset timer and swap roles of audio sources
            timer = 0f;
            AudioSource temp = audioSource1;
            audioSource1 = audioSource2;
            audioSource2 = temp;

            // Ensure audioSource1 is always the louder one
            audioSource1.volume = 1.0f;
            audioSource2.volume = 0.0f;
        }
    }
}