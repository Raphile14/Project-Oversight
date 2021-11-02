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

        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void Update()
        {
            GetInput();
            ControlDrag();
        }

        void GetInput()
        {
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");

            moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        }

        void ControlDrag()
        {
            rb.drag = Game.DEFAULT_DRAG;
        }

        private void FixedUpdate()
        {
            rb.AddForce(moveDirection.normalized * Game.DEFAULT_PLAYER_SPEED, ForceMode.Acceleration);
        }
    }

}