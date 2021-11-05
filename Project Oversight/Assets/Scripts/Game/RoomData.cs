using com.codingcatharsis.room;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
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
        private NavMeshSurface surface;

        // Start is called before the first frame update
        void Start()
        {

        }

        public int getxCoord()
        {
            return xCoord;
        }

        public int getzCoord()
        {
            return zCoord;
        }

        public void SetData(int idx, int x, int z, GameObject[] prefabs)
        {
            index = idx;
            xCoord = x;
            zCoord = z;
            roomPrefabs = prefabs;
            // Debug.Log("x: " + xCoord + " | z: " + zCoord);            
            RefillAvailableRooms();
        }

        void RefillAvailableRooms()
        {
            for (int i = 0; i < roomPrefabs.Length; i++)
            {
                roomsAvailable.Add(roomPrefabs[i]);
            }            
        }

        public void SpawnRoom()
        {
            GameObject tempFloor = gameObject.transform.Find("temp floor").gameObject;
            Destroy(tempFloor);
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
            // Debug.Log("Count: " + roomsAvailable.Count);
            int room = Random.Range(0, roomsAvailable.Count);
            // Debug.Log("Room: " + room);
            currentRoom = Instantiate((GameObject) roomsAvailable[room], this.transform);
            roomsAvailable.RemoveAt(room);

            spawnCheckers = GetComponentsInChildren<SpawnChecker>();            
        }

        public void BuildNavMesh()
        {
            // Check if room is navigationally valid
            surface = gameObject.GetComponentInChildren<NavMeshSurface>();
            surface.BuildNavMesh();
        }

        IEnumerator CheckCollision()
        {
            while (!isRoomValid)
            {
                yield return new WaitForSeconds(Game.ROOM_SPAWN_WAIT);

                bool collisionDetected = false;
                // Debug.Log(spawnCheckers.Length);

                // Check Spawner Collision
                foreach (Transform child in currentRoom.transform.Find("[Spawners]"))
                {
                    if (child.gameObject.GetComponent<SpawnChecker>().getIsColliding())
                    {
                        // Debug.Log("Detected Collision");
                        collisionDetected = true;
                        break;
                    }
                }

                // Check Wall Collision
                foreach (Transform child in currentRoom.transform.Find("[Walls]"))
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
                    GameObject control = GameObject.Find("[Game Controller]");
                    // isRoomValid = control.GetComponent<Controller>().CheckPath();
                    isRoomValid = true;

                    if (isRoomValid)
                    {
                        Debug.Log("Finished Room: " + index);
                        // Debug.Log(roomsAvailable.Count);

                        // Spawn next room
                        control.GetComponent<Controller>().SpawnRoomsWithPrefab();
                        yield break;
                    }
                }

                Destroy(currentRoom);
                // Debug.Log("Respawning");
                Spawner();
                yield return null;
            }            
        }
    }
}
