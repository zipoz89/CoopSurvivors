using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerControlls playerControlls;

    private InputAction movement;
    private InputAction shoot;
    private InputAction sprint;
    private InputAction interact;
    private InputAction skill1;
    private InputAction skill2;
    private InputAction weaponSlot1;
    private InputAction weaponSlot2;
    private InputAction weaponSlot3;
    
    public Action<bool> onSprint;
    public Action<bool> onShoot;
    public Action<bool> onInteract;
    public Action<bool> onSkill1;
    public Action<bool> onSkill2;
    public Action<bool> onWeaponSlot1;
    public Action<bool> onWeaponSlot2;
    public Action<bool> onWeaponSlot3;
    public Action<Vector2> onMovement;

    private void Awake()
    {
        playerControlls = new PlayerControlls();
        
        movement = playerControlls.Player.Movement;
        movement.performed += OnMovementChanged;
        movement.canceled += OnMovementChanged;
        
        shoot = playerControlls.Player.Shoot;
        shoot.performed += OnShootDown;
        shoot.canceled += OnShootUp;

        sprint = playerControlls.Player.Sprint;
        sprint.performed += OnSprintDown;
        sprint.canceled += OnSprintUp;
        
        interact = playerControlls.Player.Interact;
        interact.performed += OnInteractDown;
        interact.canceled += OnInteractUp;

        skill1 = playerControlls.Player.Skill1;
        skill1.performed += OnSkill1Down;
        skill1.canceled += OnSkill1Up;
        
        skill2 = playerControlls.Player.Skill2;
        skill2.performed += OnSkill2Down;
        skill2.canceled += OnSkill2Up;
        
        weaponSlot1 = playerControlls.Player.WeaponSlot1;
        weaponSlot1.performed += OnWeaponSlot1Down;
        weaponSlot1.canceled += OnWeaponSlot1Up;
        
        weaponSlot2 = playerControlls.Player.WeaponSlot2;
        weaponSlot2.performed += OnWeaponSlot2Down;
        weaponSlot2.canceled += OnWeaponSlot2Up;
        
        weaponSlot3 = playerControlls.Player.WeaponSlot3;
        weaponSlot3.performed += OnWeaponSlot3Down;
        weaponSlot3.canceled += OnWeaponSlot3Up;
    }

    private void OnEnable()
    {
        movement.Enable();
        shoot.Enable();
        sprint.Enable();
        interact.Enable();
        skill1.Enable();
        skill2.Enable();
        weaponSlot1.Enable();
        weaponSlot2.Enable();
        weaponSlot3.Enable();
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
    
    private void OnInteractDown(InputAction.CallbackContext context)
    {
        onInteract?.Invoke(true);
    }
    private void OnInteractUp(InputAction.CallbackContext context)
    {
        onInteract?.Invoke(false);
    }
    
    private void OnSkill1Down(InputAction.CallbackContext context)
    {
        onSkill1?.Invoke(true);
    }
    private void OnSkill1Up(InputAction.CallbackContext context)
    {
        onSkill1?.Invoke(false);
    }
    
    private void OnSkill2Down(InputAction.CallbackContext context)
    {
        onSkill2?.Invoke(true);
    }
    private void OnSkill2Up(InputAction.CallbackContext context)
    {
        onSkill2?.Invoke(false);
    }
    
    private void OnWeaponSlot1Down(InputAction.CallbackContext context)
    {
        onWeaponSlot1?.Invoke(true);
    }
    private void OnWeaponSlot1Up(InputAction.CallbackContext context)
    {
        onWeaponSlot1?.Invoke(false);
    }
    
    private void OnWeaponSlot2Down(InputAction.CallbackContext context)
    {
        onWeaponSlot2?.Invoke(true);
    }
    private void OnWeaponSlot2Up(InputAction.CallbackContext context)
    {
        onWeaponSlot2?.Invoke(false);
    }
    
    private void OnWeaponSlot3Down(InputAction.CallbackContext context)
    {
        onWeaponSlot3?.Invoke(true);
    }
    private void OnWeaponSlot3Up(InputAction.CallbackContext context)
    {
        onWeaponSlot3?.Invoke(false);
    }

}
