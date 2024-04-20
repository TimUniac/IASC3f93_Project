using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement; // If you're switching scenes
// using UnityEngine.UI; // If you're switching Canvases within the same scene

public class AuthCheck : MonoBehaviour
{
    // Reference to the MainMenu Canvas if switching within the same scene
    // public Canvas mainMenuCanvas;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (AuthenticationService.Instance.IsSignedIn)
        {
            // If switching to a MainMenu scene
            SceneManager.LoadScene("MainMenu");

          
        }
    }
}