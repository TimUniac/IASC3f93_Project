using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;

public class CollectibleItem : Interactable
{
    public string itemName;
    public string itemDescription;

    public Canvas canvas1;
    public Canvas canvas2;
    public Text itemNameText;
    public Text itemDescriptionText;

    public TextToAudioPlayer textToAudioPlayer;
    public TypewriterEffect typewriterEffect;

    private Renderer itemRenderer;
    private Material itemMaterial;
    private Color originalEmissionColor;
    private PlayerMotor playerMotor;
    private PlayerLook playerLook;
    [SerializeField] public int ItemIndex;
    public WhiteBoxManager wb;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        itemRenderer = GetComponent<Renderer>();
        if (player != null)
        {
            playerMotor = player.GetComponent<PlayerMotor>();
            playerLook = player.GetComponent<PlayerLook>();
        }

        if (itemRenderer != null)
        {
            itemMaterial = itemRenderer.material;
            originalEmissionColor = itemMaterial.GetColor("_EmissionColor");
        }
    }

    protected override void Interact()
    {
        if (ItemIndex == wb.GuideLocation)
        {
            wb.canAdvance = true;
        }
        Debug.Log("Collected " + itemName);

        UpdateCollectedItemsInCloud(itemName);

        if (itemMaterial != null)
        {
            itemMaterial.SetColor("_EmissionColor", Color.black);
            DynamicGI.SetEmissive(itemRenderer, Color.black);
        }

        DisablePlayerInput();
        SwitchCanvasAndUpdateText();
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

    private void DisablePlayerInput()
    {
        if (playerMotor != null) playerMotor.canMove = false;
        if (playerLook != null) playerLook.DisableLook();
    }

    private void EnablePlayerInput()
    {
        if (playerMotor != null) playerMotor.canMove = true;
        if (playerLook != null) playerLook.EnableLook();
    }

    private void SwitchCanvasAndUpdateText()
    {
        
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(true);

        itemNameText.text = itemName;
        itemDescriptionText.text = ""; // Clear existing text for typewriter effect

        if (textToAudioPlayer != null) textToAudioPlayer.ReadText(itemDescription);
        if (typewriterEffect != null) typewriterEffect.DisplayText(itemDescription); // Trigger typewriter effect
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToPlay();
        }
    }

    void ReturnToPlay()
    {
        canvas1.gameObject.SetActive(true);
        canvas2.gameObject.SetActive(false);
        EnablePlayerInput();
    }
}
