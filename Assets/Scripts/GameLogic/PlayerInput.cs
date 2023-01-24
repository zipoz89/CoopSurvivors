using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Managing.Server;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerControlls playerControlls;

    private InputAction movement;
    private InputAction shoot;
    private InputAction sprint;

    public Action<bool> onSprint;
    public Action<bool> onShoot;
    public Action<Vector2> onMovement;

    
    private void Awake()
    {
        playerControlls = new PlayerControlls();
        
        movement = playerControlls.Player.Movement;
        movement.performed += OnMovementChanged;
        movement.canceled += OnMovementChanged;
        
        shoot= playerControlls.Player.Shoot;
        shoot.performed += OnShootDown;
        shoot.canceled += OnShootUp;

        sprint = playerControlls.Player.Sprint;
        sprint.performed += OnSprintDown;
        sprint.canceled += OnSprintUp;
    }

    private void OnEnable()
    {
        movement.Enable();
        shoot.Enable();
        sprint.Enable();
    }

    private void OnMovementChanged(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        onMovement?.Invoke(direction);
    }

    private void OnShootDown(InputAction.CallbackContext context)
    {
        onShoot?.Invoke(true);
    }
    private void OnShootUp(InputAction.CallbackContext context)
    {
        onShoot?.Invoke(false);
    }
    
    private void OnSprintDown(InputAction.CallbackContext context)
    {
        onSprint?.Invoke(true);
    }
    private void OnSprintUp(InputAction.CallbackContext context)
    {
        onSprint?.Invoke(false);
    }
}
