using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

public class PlayerManager : MonoBehaviour

{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFootActions;
    private PlayerMotor motor;
    private PlayerLook look;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFootActions = playerInput.onFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
    }

    private void FixedUpdate()
    {
        motor.ProcessMove(onFootActions.Movement.ReadValue<Vector2>());
        onFootActions.Jump.performed += ctx => motor.ProcessJump();
    }
    private void LateUpdate()
    {

        look.processLook(onFootActions.Look.ReadValue<Vector2>());



    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        onFootActions.Enable();

    }

    private void OnDisable()
    {
        onFootActions.Disable();
    }
}
