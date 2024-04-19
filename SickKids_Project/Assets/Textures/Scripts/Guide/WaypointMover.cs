using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointMover : MonoBehaviour
{
    //Stores a reference to the waypoint system
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float distanceThreshold = 0.1f;
    public Canvas canvas1; // The game Canvas
    public Canvas canvas2; // The canvas of the chat
    private Transform currentWaypoint;
    public Text chatText;
    private String[] chatStrings = new []{"First Stop", "Second Stop", "Third Stop" , "Fourth Stop", "Fifth Stop"};
    private int currentIndex = 0;
    private PlayerMotor playerMotor;
    private PlayerLook playerLook;
    private GameObject player;
    public WhiteBoxManager wb;
    [SerializeField]public int startIndex = -1;
    public bool hasSpawned = false;
     
    
    // Start is called before the first frame update
    public void Start()
    {
        
       
         player= GameObject.FindWithTag("Player");
         
        // get player info
        
        if (player != null)
        {
            playerMotor = player.GetComponent<PlayerMotor>();
            playerLook = player.GetComponent<PlayerLook>(); 
        }
        //Set Initial pos

            
            currentWaypoint = waypoints.getPointOnIndex(startIndex);
            transform.position = currentWaypoint.position;
            currentIndex = waypoints.childIndex;  
        
       
        
        
        //set next waypoint target
        transform.LookAt(currentWaypoint);
       
    }


    // Update is called once per frame
    void Update()
    {

        if (hasSpawned)
        {
            
            hasSpawned = !hasSpawned;
            wb.GuideLocation = startIndex;
            waypoints.childIndex = startIndex;
            currentWaypoint = waypoints.getPointOnIndex(startIndex);
            transform.position = currentWaypoint.position;
            currentIndex = waypoints.childIndex; 
            
        }
       
        moveToNextWaypoint();
        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            transform.LookAt(player.transform);
        }
        
    }

    private void moveToNextWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

    }
    
    public void AdvanceToNextWaypoint()
    {
        ReturnToPlay();
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.LookAt(currentWaypoint);
        currentIndex++;
        wb.GuideLocation++;//last thing it should do
        if (currentIndex >= waypoints.getWaypointChildren())
        {
            currentIndex = 0;
            wb.GuideLocation = 0;
        }
        wb.UpdateGuideIndex(currentIndex);
        
    }

    public void displayInfo()
    {
       
        disableMovement();
        SwitchCanvasAndUpdateText();
    }
    
    private void SwitchCanvasAndUpdateText()
    {
        if (canvas1 != null)
        {
            chatText.text = chatStrings[currentIndex];
            canvas1.gameObject.SetActive(false);
        }

        if (canvas2 != null)
        {
            canvas2.gameObject.SetActive(true);
           
            
        }
    }
    
    public void ReturnToPlay()
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

    private void disableMovement()
    {
        if (playerMotor != null && playerLook != null)
        {
            playerMotor.canMove = false;
            playerLook.DisableLook();
        }
    }
    
  


}
