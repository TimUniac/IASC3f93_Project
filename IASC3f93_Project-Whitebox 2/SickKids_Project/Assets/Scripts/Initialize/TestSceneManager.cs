using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;

public class TestSceneManager : MonoBehaviour
{
    private string testUsername = "test_user";
    private string testPassword = "Welland2024.";
    public Text playerNameText; // Assign this in the Unity Inspector

    void Start()
    {
        InitializeServices();
    }

    async void InitializeServices()
    {
        await UnityServices.InitializeAsync();
        // Start the login process
        Login();
    }

    async void Login()
    {
        try
        {
            // Sign in with the test credentials
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(testUsername, testPassword);
                Debug.Log("Login successful");
            }

            // Proceed to load and disable found items after successful login
            LoadAndDisableFoundItems();
            LoadAndDisplayPlayerName();
            
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Login failed: " + ex.Message);
        }
    }

      async void LoadAndDisableFoundItems()
      {
          try
          {
              // Load the user's found items from Cloud Save
              var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "itemsFound" });
  
              if (playerData.TryGetValue("itemsFound", out var foundItemsObj) && foundItemsObj.Value.GetAs<List<string>>() is List<string> foundItems)
              {
                  // Iterate through the found items
                  foreach (string itemName in foundItems)
                  {
                      // Find the corresponding item in the scene
                      GameObject itemInScene = GameObject.Find(itemName);
                      if (itemInScene != null)
                      {
                          // Turn off emission for the item
                          Renderer itemRenderer = itemInScene.GetComponent<Renderer>();
                          if (itemRenderer != null)
                          {
                              Material itemMaterial = itemRenderer.material;
                              itemMaterial.SetColor("_EmissionColor", Color.black);
                              DynamicGI.SetEmissive(itemRenderer, Color.black);
                          }
                      }
                  }
              }
          }
          catch (System.Exception ex)
          {
              Debug.LogError("Error loading or processing found items: " + ex.Message);
          }
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
                    playerNameText.text = usernameValue.Value.GetAs<string>();
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
    
    
}
