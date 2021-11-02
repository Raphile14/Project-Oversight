using com.codingcatharsis.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.codingcatharsis.player
{
    public class Look : MonoBehaviour
    {
        Camera cam;
        float mouseX;
        float mouseY;

        float multiplier = 0.1f;

        float xRotation;
        float yRotation;

        private void Start()
        {
            cam = GetComponentInChildren<Camera>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            GetInput();

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        void GetInput()
        {
            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            yRotation += mouseX * Game.DEFAULT_SENSX * multiplier;
            xRotation -= mouseY * Game.DEFAULT_SENSY * multiplier;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }
    }
}

