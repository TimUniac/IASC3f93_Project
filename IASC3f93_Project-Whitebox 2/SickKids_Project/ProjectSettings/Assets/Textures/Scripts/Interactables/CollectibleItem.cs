using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using UnityEngine.InputSystem; //broken code, need to fix the disable input


public class CollectibleItem : Interactable
{
    public string itemName; // Item name to be set in the Unity Editor
    public string itemDescription;
    
    public Canvas canvas1; // Assign in the Unity Editor
    public Canvas canvas2; // Assign in the Unity Editor
    public Text itemNameText; // Assign in the Unity Editor
    public Text itemDescriptionText; // Assign in the Unity Editor
   

    private Renderer itemRenderer;
    private Material itemMaterial;
    private Color originalEmissionColor;
    private PlayerMotor playerMotor;
    private PlayerLook playerLook;
    [SerializeField] public int ItemIndex;
    public WhiteBoxManager wb;
    private bool hasInteracted;
    

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        itemRenderer = GetComponent<Renderer>();
        hasInteracted = false;
        if (player != null)
        {
            playerMotor = player.GetComponent<PlayerMotor>();
            playerLook = player.GetComponent<PlayerLook>(); 
        }

        if (playerMotor == null)
        {
            Debug.LogError("PlayerMotor component not found on the player!");
        }
        if (itemRenderer != null)
        {
            itemMaterial = itemRenderer.material;
            // Store the original emission color
            originalEmissionColor = itemMaterial.GetColor("_EmissionColor");
        }
    }

    protected override void Interact()
    {
        
        if (hasInteracted)
        {
            ReturnToPlay();
        }
        else
        {
            if (ItemIndex == wb.GuideLocation)
            {
                wb.canAdvance = true;
            }

       
            Debug.Log("Collected " + itemName);
        
        
            // Save the item name to cloud
            UpdateCollectedItemsInCloud(itemName);

            // Turn off emission to remove the glow
            if (itemMaterial != null)
            {
                itemMaterial.SetColor("_EmissionColor", Color.black);
                DynamicGI.SetEmissive(itemRenderer, Color.black);
            }
        
            // Disable player input
            if (playerMotor != null && playerLook != null)
            {
                playerMotor.canMove = false;
                playerLook.DisableLook();
            }

            SwitchCanvasAndUpdateText();
        }

        hasInteracted = !hasInteracted;
       
    }

    private async void UpdateCollectedItemsInCloud(string newItem)
    {
        try
        {
            // Load the existing list of collected items from the cloud
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "itemsFound" });
            List<string> collectedItems;

            if (playerData.TryGetValue("itemsFound", out var existingItems) && existingItems.Value.GetAs<List<string>>() is List<string> items)
            {
                collectedItems = items;
            }
            else
            {
                collectedItems = new List<string>();
            }

            // Check if the item is already in the list
            if (!collectedItems.Contains(newItem))
            {
                // Add the new item to the list since it's not there
                collectedItems.Add(newItem);

                // Save the updated list back to the cloud
                await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object> { { "itemsFound", collectedItems } });
                Debug.Log("Updated collected items in the cloud.");
            }
            else
            {
                Debug.Log($"Item '{newItem}' is already in the collected items list.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error updating collected items in the cloud: " + ex.Message);
        }
    }
    
    private void SwitchCanvasAndUpdateText()
    {
        if (canvas1 != null)
        {
            canvas1.gameObject.SetActive(false);
        }

        if (canvas2 != null)
        {
            canvas2.gameObject.SetActive(true);
            itemNameText.text = itemName; // Update item name text
            itemDescriptionText.text = itemDescription; // Update item description text
        }
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.F) )
        {
            ReturnToPlay();
        }
    }
    

void ReturnToPlay()
{
    // Enable Canvas1 and disable Canvas2
    if (canvas1 != null && canvas2 != null)
    {
        canvas1.gameObject.SetActive(true);
        canvas2.gameObject.SetActive(false);

        // Re-enable player input
        if (playerMotor != null && playerLook != null)
        {
            playerMotor.canMove = true;
            playerLook.EnableLook();
        }
    }
}
}
