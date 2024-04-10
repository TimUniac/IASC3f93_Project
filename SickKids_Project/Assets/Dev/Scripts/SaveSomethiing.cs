using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine.UI; // Import the UI namespace
using UnityEngine.SceneManagement;


public class SaveSomethiing : MonoBehaviour
{
    public InputField usernameInputField; // Reference to the username input field
    public InputField passwordInputField; // Reference to the password input field
    public InputField itemInputField; // Reference to the item input field
    private Dictionary<string, object> playerData;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
   
       
    }
    

    public async void SignUpWithUsernamePassword()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        try
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                AuthenticationService.Instance.SignOut();
            }

            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");

            // Initialize player data upon successful signup
            var initialPlayerData = new Dictionary<string, object>
            {
                { "username", username },
                { "itemsFound", new List<string>() }, // Empty list for items found
                { "questsCompleted", new List<string>() }, // Empty list for quests completed
                { "GuideIndex", 0 }
            };

            await CloudSaveService.Instance.Data.Player.SaveAsync(initialPlayerData);
            Debug.Log("Initialized player data for new user: "+ username);

            // Load the MainMenu scene after successful signup
            SceneManager.LoadScene("MainMenu");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during sign up or initializing data: {ex.Message}");
        }
    }

    
    public async void SignInWithUsernamePasswordAsync()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
    
        try
        {
            AuthenticationService.Instance.SignOut();
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");

            // Load the MainMenu scene after successful sign-in
            SceneManager.LoadScene("MainMenu");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    
    public async void AddItemToFoundItems()
    {
        // Use the text from the input field as the item name
        string item = itemInputField.text;

        try
        {
            // Load the current "itemsFound" list
            var keysToLoad = new HashSet<string> { "itemsFound" };
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(keysToLoad);

            if (playerData.TryGetValue("itemsFound", out var itemsFoundObj) && itemsFoundObj.Value.GetAs<List<string>>() is List<string> itemsFoundList)
            {
                // Add the new item
                itemsFoundList.Add(item);

                // Prepare the updated data
                var updatedData = new Dictionary<string, object> { { "itemsFound", itemsFoundList } };

                // Save the updated player data
                await CloudSaveService.Instance.Data.Player.SaveAsync(updatedData);
                Debug.Log($"Item '{item}' added to the player's found items.");
            }
            else
            {
                Debug.LogError("Failed to retrieve or parse 'itemsFound' list.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to add item to 'itemsFound': " + ex.Message);
        }
    }






}