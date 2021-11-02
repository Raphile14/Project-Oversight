using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.codingcatharsis.menu
{    

    public class Menu : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject settingsMenu;

        public void StartGame()
        {
            SceneManager.LoadScene("MainGame");
        }

        public void QuitGame()
        {

        }

        public void ToggleSettings()
        {
            mainMenu.SetActive(!mainMenu.activeSelf);
            settingsMenu.SetActive(!settingsMenu.activeSelf);
        }        
    }
}
