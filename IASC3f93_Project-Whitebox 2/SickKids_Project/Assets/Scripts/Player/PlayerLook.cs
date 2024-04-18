using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera Cam;
    public float xRotation = 0;
    public float ySensitivity = 30f;
    public float xSensitivity = 30f;

    private bool canLook = true; // Add this line

    public void processLook(Vector2 input)
    {
        if (!canLook) return; // Add this line

        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        Cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    // Add these methods to control the canLook flag
    public void EnableLook()
    {
        canLook = true;
    }

    public void DisableLook()
    {
        canLook = false;
    }
}