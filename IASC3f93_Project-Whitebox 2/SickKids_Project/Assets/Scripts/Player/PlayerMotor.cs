 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController characterController;
    private Vector3 PlayerVelocity;
    public float speed = 5f;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 3.0f;
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        if (!canMove) return;
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        characterController.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        PlayerVelocity.y += (gravity * Time.deltaTime);
        if (isGrounded && PlayerVelocity.y < 0)
        {
            PlayerVelocity.y = -2f;
        }
        characterController.Move(PlayerVelocity * Time.deltaTime);

    }
    public void ProcessJump()
    {
        if (!canMove || !isGrounded) return;
        if (isGrounded)
        {
            PlayerVelocity.y = (float)Math.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
