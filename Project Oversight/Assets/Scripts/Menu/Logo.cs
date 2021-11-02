using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace com.codingcatharsis.menu
{
    public class Logo : MonoBehaviour
    {
        public float waitTime = 4.0f;
        public VideoPlayer player;

        void Start()
        {
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, "intro.mp4");
            player.url = path;

            player.targetCameraAlpha = 1.0f;
            player.Play();
            StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
