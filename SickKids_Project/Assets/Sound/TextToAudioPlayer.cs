using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToAudioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float playbackInterval = -0.2f; // Play with this till it sounds right. 
    private AudioSource audioSource;
    private Queue<AudioClip> playQueue = new Queue<AudioClip>(); 
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadAudioClips();
    }

    void LoadAudioClips()
    {
        audioClips = new AudioClip[91];
        for (int i = 1; i <= 91; i++)
        {
            audioClips[i - 1] = Resources.Load<AudioClip>($"RoboSyllabs/RoboVoice_{i}");
        }
    }

    public void ReadText(string text) // feed text using:  string text = "Hello, world!";
                                      //                   textToAudioPlayer.ReadText(text);
    {
        int charCount = text.Length;
        int clipsToPlayCount = Mathf.Max(1, Mathf.CeilToInt(charCount / 6.0f * 1.0f));
        List<AudioClip> clipsToPlay = new List<AudioClip>();
        
        for (int i = 0; i < clipsToPlayCount; i++)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            clipsToPlay.Add(audioClips[randomIndex]);
        }

        StartCoroutine(PlayAudioClips(clipsToPlay));
        Debug.Log($"Enqueued {clipsToPlay.Count} clips for playback.");
    }

    IEnumerator PlayAudioClips(List<AudioClip> clips)
    {
        foreach (AudioClip clip in clips)
        {
            playQueue.Enqueue(clip);
            while (playQueue.Count > 0)
            {
                if (!audioSource.isPlaying)
                {
                    AudioClip clipToPlay = playQueue.Dequeue();
                    audioSource.clip = clipToPlay;

                    audioSource.pitch = 1.0f + Random.Range(-0.05f, 0.05f);//also play with this to fit desired effect

                    audioSource.Play();
                    yield return new WaitForSeconds(playbackInterval);
                }
                else
                {
                    yield return null;
                }
            }
            audioSource.pitch = 1.0f;
        }
    }
}