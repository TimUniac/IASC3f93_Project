using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WhiteBoxManager : MonoBehaviour
{
    private string testUsername = "test_user";
    private string testPassword = "Welland2024.";
    public Text playerNameText; // Assign this in the Unity Inspector
    [SerializeField] public bool canAdvance;
    public int GuideLocation;
    public GameObject pauseMenu;
    private bool isPaused;
    

   

    public void Start()
    {
        InitializeServices();
        canAdvance = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            PauseGame();
        }
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
            
            LoadAndDisplayPlayerName();
            LoadAndDisableFoundItems();
            LoadGuideIndexAndSetStartIndex();



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
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "foundItems" });

            if (playerData.TryGetValue("foundItems", out var foundItemsObj) && foundItemsObj.Value.GetAs<List<string>>() is List<string> foundItems)
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
        
        public async void UpdateGuideIndex(int newIndex)
        {
            try
            {
                // Update the GuideIndex value in Cloud Save
                await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object> { { "GuideIndex", newIndex } });
                Debug.Log($"GuideIndex updated to {newIndex} in Cloud Save.");
                
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to update GuideIndex: {ex.Message}");
            }
        }
        
        //says if player can advance to next AI Scene
        public void negAdvance()
        {
            canAdvance = !canAdvance;
        }
        public async void ExitGame()
        {
            // Log message to console
            Debug.Log("Exiting Game");
        
            // Quit the application
            Application.Quit();
        }
        
        public async void PauseGame()
        {
            if (!isPaused)
            {
                pauseMenu.SetActive(true);
            }
            else
            {
                pauseMenu.SetActive(false);
            }

            isPaused = !isPaused;
        }
        
        public async void LoadGuideIndexAndSetStartIndex()
        {
            try
            {
                // Load the GuideIndex value from Cloud Save
                var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "GuideIndex" });

                if (playerData.TryGetValue("GuideIndex", out var guideIndexValue))
                {
                    int guideIndex = guideIndexValue.Value.GetAs<int>();
                    Debug.Log($"Fetched GuideIndex: {guideIndex}");

                    // Find the guide GameObject in the scene
                    GameObject guideObject = GameObject.Find("Guide");
                    if (guideObject != null)
                    {
                        // Get the WaypointMover component
                        WaypointMover waypointMover = guideObject.GetComponent<WaypointMover>();
                        if (waypointMover != null)
                        {
                            // Set the startIndex variable
                            waypointMover.startIndex = guideIndex;
                            waypointMover.hasSpawned = true;
                            Debug.Log("Guide's startIndex set to: " + guideIndex);
                        }
                        else
                        {
                            Debug.LogError("WaypointMover component not found on 'guide' GameObject.");
                        }
                    }
                    else
                    {
                        Debug.LogError("'guide' GameObject not found in the scene.");
                    }
                }
                else
                {
                    Debug.LogError("GuideIndex not found in Cloud Save data.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load and set GuideIndex: {ex.Message}");
            }
        }

        
        

  
}
