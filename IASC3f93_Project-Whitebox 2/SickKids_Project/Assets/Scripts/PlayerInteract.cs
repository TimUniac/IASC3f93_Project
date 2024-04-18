using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerManager inputManager;
    private PLayerUI playerUI;
    
    void Start()
    {
        cam = GetComponent<PlayerLook>().Cam;
        playerUI = GetComponent<PLayerUI>();
        inputManager = GetComponent<PlayerManager>();

    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(String.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward); 
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                if (inputManager.onFootActions.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }

    }
}
