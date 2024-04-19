using System.Collections;
using UnityEngine;

public class TextToAudioPlayer : MonoBehaviour
{   [SerializeField]
    public AudioClip[] audioClips; // Array to hold all audio clips
    private AudioSource audioSource; // AudioSource component attached to the same GameObject

    void Start()
    {
        // Get the AudioSource component from the GameObject this script is attached to
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Missing AudioSource component on the GameObject.");
            return; // If AudioSource is not found, log an error and stop further execution
        }

        // Load audio clips; assuming this method will properly populate the audioClips array
        LoadAudioClips();
    }

    void LoadAudioClips()
    {
        audioClips = new AudioClip[83]; 
        for (int i = 1; i <= 83; i++)
        {
            // Construct the path for each clip
            string path = $"Audio/RoboVoice_{i}";
            audioClips[i - 1] = Resources.Load<AudioClip>(path);
            if (audioClips[i - 1] == null)
            {
                Debug.LogError($"Failed to load AudioClip at path: {path}");
            }
        }
    }

    public void ReadText(string text)
    {
        int charCount = text.Length;
        int clipsToPlayCount = Mathf.Max(1, Mathf.CeilToInt(charCount / 7.0f)); // Calculate how many clips to play
        StartCoroutine(PlayAudioClipsRandomly(clipsToPlayCount));
    }

    IEnumerator PlayAudioClipsRandomly(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (audioClips.Length == 0)
            {
                Debug.LogError("Audio clips array is empty.");
                yield break; // Exit if no clips are loaded
            }

            // Select a random clip to play
            AudioClip clipToPlay = audioClips[Random.Range(0, audioClips.Length)];
            if (clipToPlay == null)
            {
                Debug.LogError("Encountered a null AudioClip in the array.");
                continue; // Skip this iteration if the clip is null
            }

            // Play the selected audio clip
            audioSource.pitch = 1.0f + Random.Range(-0.05f, 0.05f); // Slightly vary the pitch
            audioSource.PlayOneShot(clipToPlay);
            yield return new WaitForSeconds(clipToPlay.length + 0.05f); // Wait for the clip to finish plus an interval
        }

        // Reset the pitch after all clips are played
        audioSource.pitch = 1.0f;
    }
}