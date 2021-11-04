using com.codingcatharsis.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.codingcatharsis.player
{
    public class Movement : MonoBehaviour
    {
        Vector3 moveDirection;
        float horizontalMovement;
        float verticalMovement;
        bool isRunning = false;

        Rigidbody rb;

        private void Start()
        {
            Game.SetDefaults();
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;            
        }

        private void Update()
        {
            GetInput();
            ControlDrag();
            Debug.Log("Health: " + Game.currentHealth + " Stamina: " + Game.currentStamina);
        }

        void GetInput()
        {
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");

            moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;

            // Check if Sprinting
            if (Input.GetKey(KeyCode.LeftShift) && (horizontalMovement != 0 || verticalMovement != 0) && Game.currentStamina > 0)
            {
                isRunning = true;
                Game.currentStamina -= Game.STAMINA_CONSUMPTION * Time.deltaTime;
            }
            else
            {
                isRunning = false;
            }

            // Regen Stamina if not sprinting
            if (!Input.GetKey(KeyCode.LeftShift) && Game.currentStamina < Game.DEFAULT_MAX_STAMINA)
            {
                Game.currentStamina += Game.STAMINA_REGENERATION * Time.deltaTime;
            }
        }

        void ControlDrag()
        {
            rb.drag = Game.DEFAULT_DRAG;
        }

        private void FixedUpdate()
        {
            float speed = Game.DEFAULT_PLAYER_SPEED;

            if (isRunning)
            {
                speed = Game.DEFAULT_PLAYER_SPRINTSPEED;
            }

            rb.AddForce(moveDirection.normalized * speed, ForceMode.Acceleration);
        }
    }

}