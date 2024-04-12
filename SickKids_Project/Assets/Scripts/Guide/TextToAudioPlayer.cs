using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TextToAudioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; // To hold all audio clips
    public float playbackInterval = 0.2f; // Adjustable playback interval in seconds, should be positive
    private AudioSource audioSource;

    void Start()
    {
          audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
        Debug.LogError("Missing AudioSource component on the GameObject.");
        return; // Add return to prevent further execution if AudioSource is not found
    }
    LoadAudioClips();
    }

 void LoadAudioClips()
{
    audioClips = new AudioClip[91]; // Adjust the number based on your actual clips
    for (int i = 1; i <= 91; i++)
    {
        audioClips[i - 1] = Resources.Load<AudioClip>($"RoboSyllabs/RoboVoice_{i}");
        if (audioClips[i - 1] == null)
         {
            Debug.LogError("Failed to load AudioClip at index " + (i-1));
        }
        }// Assuming the clips are statically assigned via the Unity Editor or loaded once and reused
    }

    public void ReadText(string text)
    {
        int charCount = text.Length;
        int clipsToPlayCount = Mathf.Max(1, Mathf.CeilToInt(charCount / 6.0f));
        StartCoroutine(PlayAudioClipsRandomly(clipsToPlayCount));
    }

    IEnumerator PlayAudioClipsRandomly(int count)
    {
        for (int i = 0; i < count; i++)
    {
        if (audioClips.Length == 0 || audioClips.Any(clip => clip == null))
        {
            Debug.LogError("Audio clips array is empty or contains null entries.");
            yield break; // Exit if there are invalid clips
        }

        int randomIndex = Random.Range(0, audioClips.Length);
        AudioClip clipToPlay = audioClips[randomIndex];
        if (clipToPlay == null)
        {
            Debug.LogError($"Null AudioClip at index {randomIndex}");
            continue; // Skip this iteration if the clip is null
        }

        audioSource.pitch = 1.0f + Random.Range(-0.05f, 0.05f);
        audioSource.PlayOneShot(clipToPlay);
        yield return new WaitForSeconds(clipToPlay.length + playbackInterval);
    }
    audioSource.pitch = 1.0f; // Reset pitch after playing all clips
    }
}
