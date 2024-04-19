using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using UnityEngine.SceneManagement;

public class PatientRoom1 : MonoBehaviour
{
    public Text monitorChat;
    public GameObject botTextBox;
    private bool hasInteracted;
    public GameObject pauseMenu;
    private bool isPaused;
    
    
    // Start is called before the first frame update
    void Start()
    {
        hasInteracted = false;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            PauseGame();
        }
    }

    private void interacted()
    {
        if (!hasInteracted)
        {
            botTextBox.gameObject.SetActive(true);
           
        }
        else
        {
            botTextBox.gameObject.SetActive(false);  
        }
        hasInteracted = !hasInteracted;
    }

    public async void MonitorChat()
    {
        interacted();
        monitorChat.text = "This is a test";
        
        
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

    public async void ReturnToHospital()
    {
        SceneManager.LoadScene("Whitebox");
    }

    public async void CloseGame()
    {
        Application.Quit();
    }
    
    
}
