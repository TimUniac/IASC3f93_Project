using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private TypewriterEffect typewriterEffect;

    [SerializeField]
    private TextToAudioPlayer audioPlayer; // Reference to your audio player script

    [SerializeField]
    private List<LocationDialogue> locationDialogues; // List of dialogues based on location

    private Dictionary<string, LocationDialogue> dialogueDictionary = new Dictionary<string, LocationDialogue>(); // Dictionary to optimize location lookup

    void Start()
    {
        // Populate the dictionary from the serialized list for quicker access during gameplay
        foreach (var locationDialogue in locationDialogues)
        {
            if (!dialogueDictionary.ContainsKey(locationDialogue.locationName))
            {
                dialogueDictionary.Add(locationDialogue.locationName, locationDialogue);
            }
        }
    }

    void UpdateDialogueBasedOnLocation(Vector3 playerPosition)
    {
        string currentLocation = DetermineLocation(playerPosition);
        
        // Using dictionary to quickly find the relevant dialogue based on location
        if (dialogueDictionary.TryGetValue(currentLocation, out LocationDialogue locationDialogue))
        {
            typewriterEffect.DisplayText(locationDialogue.dialogueText);
            audioPlayer.ReadText(locationDialogue.dialogueText);
        }
    }

    private string DetermineLocation(Vector3 playerPosition)
    {
        // Implement logic to determine location name based on playerPosition
        return "ExampleLocation"; // Placeholder for example
    }
}

[System.Serializable]
public class LocationDialogue
{
    public string locationName;
    public string dialogueText;
}
