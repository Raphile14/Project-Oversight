using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.codingcatharsis.internalroom
{
    public class Exit : MonoBehaviour
    {
        [SerializeField]
        private bool isGameFinished = true;

        private void OnTriggerEnter(Collider other)
        {
            if (isGameFinished && other.tag == "Player")
            {
                Cursor.lockState = CursorLockMode.Confined;
                SceneManager.LoadScene("WinScene");
            }
        }

        public void gameIsFinished()
        {
            isGameFinished = true;
        }
    }
}
