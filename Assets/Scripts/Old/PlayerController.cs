using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Old
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkingSpeed = 1;
        [SerializeField] private float runningSpeedMultiplier = 1.5f;

        [SerializeField] private float maxStamina = 5;
        private float currentStaminaUsage = 0;

        private float horizontalInput;
        private float verticalInput;
        private bool sprint;
        private Vector2 mouseDirection;
        private Camera myCamera;

        [SerializeField] private PlayerGun gun;

        void Update()
        {
            HandleInput();
            HandleMovement();
            HandleGun();
        }

        public void SetCamera(Camera camera)
        {
            myCamera = camera;
            var camTransform = myCamera.transform;

            camTransform.parent = this.transform;
            camTransform.position = new Vector3(0, 0, -10);
        }

        public void SetOtherPlayer()
        {
            this.enabled = false;
        }

        private void HandleInput()
        {
            horizontalInput = 0;
            verticalInput = 0;

            if (Input.GetKey(KeyCode.A))
            {
                horizontalInput -= 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                horizontalInput += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                verticalInput -= 1;
            }

            if (Input.GetKey(KeyCode.W))
            {
                verticalInput += 1;
            }

            if (myCamera)
            {
                Vector3 mousePosOnScreen = Input.mousePosition;
                mousePosOnScreen.z = 10;
                mouseDirection = (Camera.main.ScreenToWorldPoint(mousePosOnScreen) - this.transform.position)
                    .normalized;
            }

            sprint = Input.GetKey(KeyCode.LeftShift);
        }

        private void HandleMovement()
        {
            Vector2 movementDir = new Vector2(horizontalInput, verticalInput).normalized;

            float speed = sprint ? walkingSpeed * runningSpeedMultiplier : walkingSpeed;

            this.transform.position += (Vector3)movementDir * (Time.deltaTime * speed);
        }

        private void HandleGun()
        {
            gun.SetRotation(mouseDirection);
        }

    }
}