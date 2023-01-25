using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkingSpeed = 1;
    [SerializeField] private float sprintSpeedMultiplier = 1.5f;
    
    //input values
    private bool isSprinting = false;
    private Vector2 movementDirection = Vector2.zero;

    public void RegisterInput(PlayerInput input)
    {
        input.onSprint += SetSprint;
        input.onMovement += SetMovement;
    }

    private void SetSprint(bool state)
    {
        isSprinting = state;
    }

    private void SetMovement(Vector2 state)
    {
        movementDirection = state.normalized;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float speed = isSprinting ? walkingSpeed * sprintSpeedMultiplier : walkingSpeed;

        this.transform.position += (Vector3)movementDirection * (Time.deltaTime * speed);
    }
}
