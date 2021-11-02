using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.codingcatharsis.game
{
    public class RoomData : MonoBehaviour
    {
        private int xCoord, zCoord;
        private ArrayList roomsAvailable = new ArrayList();
        private GameObject[] roomPrefabs;
        private GameObject currentRoom;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetData(int x, int z, GameObject[] prefabs)
        {
            xCoord = x;
            zCoord = z;
            roomPrefabs = prefabs;
            // Debug.Log("x: " + xCoord + " | z: " + zCoord);

            for (int i = 0; i < prefabs.Length; i++)
            {
                roomsAvailable.Add(prefabs[i]);
            }
            SpawnRoom();
        }

        public void SpawnRoom()
        {
            if (currentRoom) Destroy(currentRoom);
            // Debug.Log(roomsAvailable.Count);
            // Choose an available room
            int room = Random.Range(0, roomsAvailable.Count);
            Instantiate(roomPrefabs[room], this.transform);
            roomsAvailable.RemoveAt(room);

            // Debug.Log(roomsAvailable.Count);
        }
    }
}
