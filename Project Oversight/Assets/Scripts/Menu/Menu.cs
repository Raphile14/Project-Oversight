using com.codingcatharsis.game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.codingcatharsis.menu
{    

    public class Menu : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject settingsMenu;
        public GameObject playMenu;
        public TMP_InputField seedInput;

        public void StartGame()
        {
            if (seedInput.text.Length > 0)
            {
                Game.SetSeed(int.Parse(seedInput.text));
            }            
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

        public void TogglePlayGame()
        {
            mainMenu.SetActive(!mainMenu.activeSelf);
            playMenu.SetActive(!playMenu.activeSelf);
        }
    }
}
