using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.codingcatharsis.game
{
    public class Controller : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject[] roomPrefabs;
        public GameObject boundaryWallPrefab;
        public GameObject boundaryWallContainer;
        public GameObject roomContainer;
        public GameObject entityContainer;        
        public GameObject enemyContainer;

        // Instantiated Objects
        GameObject player;
        GameObject[] rooms;

        // Barrier GameObjects
        GameObject northBWall, southBWall, westBWall, eastBWall;

        // Spawning Stats
        int currentRoom = 0;

        void Start()
        {
            SpawnBoundaryWalls();
            SpawnRooms();
            SpawnRoomsWithPrefab();
            // DeactivateBoundaryWalls();
            SpawnPlayer();
        }

        void SpawnBoundaryWalls()
        {
            northBWall = Instantiate(boundaryWallPrefab, boundaryWallContainer.transform);
            northBWall.name = "North";
            northBWall.transform.localScale = new Vector3(Game.ROOM_WIDTH * Game.MAP_WIDTH, 5, 0.25f);
            northBWall.transform.localPosition = new Vector3(((Game.ROOM_WIDTH * Game.MAP_WIDTH) / 2) - (Game.ROOM_WIDTH / 2), 3, Game.ROOM_HEIGHT * Game.MAP_HEIGHT - (Game.ROOM_HEIGHT / 2));

            southBWall = Instantiate(boundaryWallPrefab, boundaryWallContainer.transform);
            southBWall.transform.localScale = new Vector3(Game.ROOM_WIDTH * Game.MAP_WIDTH, 5, 0.25f);
            southBWall.name = "South";
            southBWall.transform.localPosition = new Vector3(((Game.ROOM_WIDTH * Game.MAP_WIDTH) / 2) - (Game.ROOM_WIDTH / 2), 3, -(Game.ROOM_HEIGHT / 2));

            westBWall = Instantiate(boundaryWallPrefab, boundaryWallContainer.transform);
            westBWall.transform.localScale = new Vector3(0.25f, 5, Game.ROOM_HEIGHT * Game.MAP_HEIGHT);
            westBWall.name = "West";
            westBWall.transform.localPosition = new Vector3(-(Game.ROOM_WIDTH / 2), 3, ((Game.ROOM_HEIGHT * Game.MAP_HEIGHT) / 2) - (Game.ROOM_WIDTH / 2));

            eastBWall = Instantiate(boundaryWallPrefab, boundaryWallContainer.transform);
            eastBWall.transform.localScale = new Vector3(0.25f, 5, Game.ROOM_HEIGHT * Game.MAP_HEIGHT);
            eastBWall.name = "East";
            eastBWall.transform.localPosition = new Vector3((Game.ROOM_WIDTH * Game.MAP_WIDTH) - (Game.ROOM_HEIGHT / 2), 3, ((Game.ROOM_HEIGHT * Game.MAP_HEIGHT) / 2) - (Game.ROOM_WIDTH / 2));
        }

        void DeactivateBoundaryWalls()
        {
            northBWall.SetActive(false);
            southBWall.SetActive(false);
            westBWall.SetActive(false);
            eastBWall.SetActive(false);
        }

        void SpawnRooms()
        {
            rooms = new GameObject[Game.MAP_WIDTH * Game.MAP_HEIGHT];
            for (int i = 0, z = 0; z < Game.MAP_HEIGHT; z++)
            {
                for (int x = 0; x < Game.MAP_WIDTH; x++)
                {
                    GameObject obj = new GameObject("x: " + x + " z: " + z);
                    obj.transform.SetParent(roomContainer.transform);

                    // Add and Set RoomData component
                    obj.AddComponent<RoomData>();                    
                    obj.GetComponent<RoomData>().SetData(i, x, z, roomPrefabs);

                    rooms[i] = obj;
                    rooms[i].transform.localPosition = new Vector3(x * Game.ROOM_WIDTH, 0, z * Game.ROOM_HEIGHT);
                    i += 1;
                }
            }
        }

        public void SpawnRoomsWithPrefab()
        {
            if (currentRoom < rooms.Length)
            {
                rooms[currentRoom].GetComponent<RoomData>().SpawnRoom();
                currentRoom += 1;
            }            
        }

        void SpawnPlayer()
        {
            player = Instantiate(playerPrefab, entityContainer.transform);
        }
    }
}
