using com.codingcatharsis.room;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.codingcatharsis.game
{
    public class RoomData : MonoBehaviour
    {
        private int index, xCoord, zCoord;
        private ArrayList roomsAvailable = new ArrayList();
        private GameObject[] roomPrefabs;
        private GameObject currentRoom;
        private SpawnChecker[] spawnCheckers;
        private IEnumerator coroutine;
        private bool isRoomValid = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetData(int idx, int x, int z, GameObject[] prefabs)
        {
            index = idx;
            xCoord = x;
            zCoord = z;
            roomPrefabs = prefabs;
            // Debug.Log("x: " + xCoord + " | z: " + zCoord);

            for (int i = 0; i < prefabs.Length; i++)
            {
                roomsAvailable.Add(prefabs[i]);
            }            
        }

        public void SpawnRoom()
        {
            Spawner();
            coroutine = CheckCollision();
            StartCoroutine(coroutine);
            // Debug.Log(roomsAvailable.Count);
        }

        private void Spawner()
        {
            // Debug.Log(roomsAvailable.Count);
            // Debug.Log("Spawning");
            // Choose an available room
            int room = Random.Range(0, roomsAvailable.Count);
            currentRoom = Instantiate(roomPrefabs[room], this.transform);
            roomsAvailable.RemoveAt(room);

            spawnCheckers = GetComponentsInChildren<SpawnChecker>();            
        }

        IEnumerator CheckCollision()
        {
            while (!isRoomValid)
            {
                // if (!isRoomValid) Spawner();
                yield return new WaitForSeconds(Game.ROOM_SPAWN_WAIT);

                bool collisionDetected = false;
                // Debug.Log(spawnCheckers.Length);

                foreach (Transform child in currentRoom.transform.Find("[Spawners]"))
                {
                    if (child.gameObject.GetComponent<SpawnChecker>().getIsColliding())
                    {
                        // Debug.Log("Detected Collision");
                        collisionDetected = true;
                        break;
                    }
                }

                if (!collisionDetected)
                {
                    // Debug.Log("Breaking");
                    Debug.Log("Finished Room: " + index);
                    Debug.Log(roomsAvailable.Count);
                    isRoomValid = true;

                    // Spawn next room
                    GameObject control = GameObject.Find("[Game Controller]");
                    control.GetComponent<Controller>().SpawnRoomsWithPrefab();
                    yield break;
                }

                else
                {
                    Destroy(currentRoom);
                    // Debug.Log("Respawning");
                    Spawner();
                    yield return null;
                }
            }            
        }
    }
}
