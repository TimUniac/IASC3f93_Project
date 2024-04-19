using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotInteract : Interactable
{
    private WaypointMover waypointMover;
    private bool isInteracted = false;
    public WhiteBoxManager wb;

    void Start()
    {
        // Get the WaypointMover component from this GameObject
        waypointMover = GetComponent<WaypointMover>();
    }

    protected override void Interact()
    {
        if (waypointMover != null)
        {
            if (!isInteracted && !wb.canAdvance)
            {
                waypointMover.displayInfo();
            }
            else if (isInteracted && !wb.canAdvance)
            {
                waypointMover.ReturnToPlay();
            }
            else if (isInteracted && wb.canAdvance)
            {
                waypointMover.ReturnToPlay();
            }
            else if (!isInteracted && wb.canAdvance)
            {
                waypointMover.AdvanceToNextWaypoint();
                wb.negAdvance();
            }
            isInteracted = !isInteracted; // Toggle the interacted state
        }
    }
}