using com.codingcatharsis.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.codingcatharsis.room
{
    public class SpawnChecker : MonoBehaviour
    {
        private bool isColliding = false;
        private void OnTriggerEnter(Collider other)
        {
            if ((gameObject.tag == "Checker" && (other.tag == "Boundary Wall" || other.tag == "Wall")) 
                || (gameObject.tag == "Wall" && other.tag == "Checker"))
            {
                // Debug.Log(other.tag);
                isColliding = true;
                // gameObject.GetComponentInParent<RoomData>().SpawnRoom();
                // this.gameObject.SetActive(false);
            }
        }

        public bool getIsColliding()
        {
            return isColliding;
        }
    }
}
