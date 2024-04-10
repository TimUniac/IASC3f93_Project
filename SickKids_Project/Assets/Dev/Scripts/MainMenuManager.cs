using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    public Text playerNameText; // Assign this in the Unity Inspector

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        // Load and display the player's name
        LoadAndDisplayPlayerName();
    }

    private async void LoadAndDisplayPlayerName()
    {
        try
        {
            // Define the keys you want to load
            var keysToLoad = new HashSet<string> { "username" };
            
            // Load the data from Cloud Save
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(keysToLoad);

            // Check if the "username" key exists and retrieve its value
            if (playerData.TryGetValue("username", out var usernameValue))
            {
                // Set the Text component's text to the username
                playerNameText.text ="Player Name: " + usernameValue.Value.GetAs<string>();
            }
            else
            {
                Debug.LogError("Username not found in Cloud Save data.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to load player name: " + ex.Message);
        }
    }
    
    public void LoadWhiteboxScene()
    {
        SceneManager.LoadScene("Whitebox");
    }
}