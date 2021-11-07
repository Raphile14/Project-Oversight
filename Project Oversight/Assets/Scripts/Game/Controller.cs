using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace com.codingcatharsis.game
{
    public class Controller : MonoBehaviour
    {
        // Prefabs
        public GameObject playerPrefab, floorPrefab, roofPrefab, hallwayFloorPrefab, boundaryWallPrefab;
        public GameObject[] roomPrefabs, internalRoomPrefabs;
        public GameObject spawnRoom, exitRoom;

        // Containers
        public GameObject hallwayContainer, boundaryWallContainer, roomContainer, entityContainer, enemyContainer;

        // Internal Room Objects

        // Instantiated Objects
        GameObject player;
        GameObject[] rooms;

        // Barrier and Hallway Floor GameObjects
        GameObject northBWall, southBWall, westBWall, eastBWall, hallwayFloor;

        // Spawning Stats
        int currentRoom = 0;

        void Start()
        {
            SpawnBoundaryWalls();
            SpawnRooms();
            SpawnRoomsWithPrefab();
        }

        void SpawnBoundaryWalls()
        {
            hallwayFloor = Instantiate(hallwayFloorPrefab, hallwayContainer.transform);
            hallwayFloor.name = "Entire Hallway Floor";
            hallwayFloor.transform.localScale = new Vector3(
                // x
                (Game.ROOM_WIDTH * Game.MAP_WIDTH) + (Game.HALLWAY_WIDTH * (Game.MAP_WIDTH - 1)),
                // y
                1,
                // z
                (Game.ROOM_HEIGHT * Game.MAP_HEIGHT) + (Game.HALLWAY_WIDTH * (Game.MAP_HEIGHT - 1))
                );
            hallwayFloor.transform.localPosition = new Vector3(
                // x
                (((Game.ROOM_WIDTH * Game.MAP_WIDTH) / 2) + ((Game.HALLWAY_WIDTH * (Game.MAP_WIDTH - 1)) / 2) - (Game.ROOM_WIDTH / 2)),
                // y
                -0.05f,
                // z
                (((Game.ROOM_HEIGHT * Game.MAP_HEIGHT) / 2) + ((Game.HALLWAY_WIDTH * (Game.MAP_HEIGHT - 1)) / 2) - (Game.ROOM_HEIGHT / 2))
                );

            northBWall = Instantiate(boundaryWallPrefab, boundaryWallContainer.transform);
            northBWall.name = "North";
            northBWall.transform.localScale = new Vector3((Game.ROOM_WIDTH * Game.MAP_WIDTH) + (Game.HALLWAY_WIDTH * (Game.MAP_WIDTH - 1)), 5, 0.25f);
            northBWall.transform.localPosition = new Vector3(
                // x
                ((Game.ROOM_WIDTH * Game.MAP_WIDTH) / 2) + ((Game.HALLWAY_WIDTH * (Game.MAP_WIDTH - 1)) / 2) - (Game.ROOM_WIDTH / 2),
                // y
                3, 
                // z
                (Game.ROOM_HEIGHT * Game.MAP_HEIGHT) + (Game.HALLWAY_WIDTH * (Game.MAP_WIDTH - 1)) - (Game.ROOM_HEIGHT / 2));

            southBWall = Instantiate(boundaryWallPrefab, boundaryWallContainer.transform);
            southBWall.transform.localScale = new Vector3((Game.ROOM_WIDTH * Game.MAP_WIDTH) + (Game.HALLWAY_WIDTH * (Game.MAP_WIDTH - 1)), 5, 0.25f);
            southBWall.name = "South";
            southBWall.transform.localPosition = new Vector3(
                // x
                ((Game.ROOM_WIDTH * Game.MAP_WIDTH) / 2) + ((Game.HALLWAY_WIDTH * (Game.MAP_WIDTH - 1)) / 2) - (Game.ROOM_WIDTH / 2), 
                // y
                3, 
                // z
                -(Game.ROOM_HEIGHT / 2));

            // USE AS BASIS
            westBWall = Instantiate(boundaryWallPrefab, boundaryWallContainer.transform);
            westBWall.transform.localScale = new Vector3(0.25f, 5, (Game.ROOM_HEIGHT * Game.MAP_HEIGHT) + (Game.HALLWAY_WIDTH * (Game.MAP_HEIGHT - 1)));
            westBWall.name = "West";
            westBWall.transform.localPosition = new Vector3(
                // x
                -(Game.ROOM_WIDTH / 2),
                // y
                3,
                // z
                ((Game.ROOM_HEIGHT * Game.MAP_HEIGHT) / 2) + ((Game.HALLWAY_WIDTH * (Game.MAP_HEIGHT - 1)) / 2) - (Game.ROOM_HEIGHT / 2)
            );

            eastBWall = Instantiate(boundaryWallPrefab, boundaryWallContainer.transform);
            eastBWall.transform.localScale = new Vector3(0.25f, 5, (Game.ROOM_HEIGHT * Game.MAP_HEIGHT) + (Game.HALLWAY_WIDTH * (Game.MAP_HEIGHT - 1)));
            eastBWall.name = "East";
            eastBWall.transform.localPosition = new Vector3(
                // x
                (Game.ROOM_WIDTH * Game.MAP_WIDTH) - (Game.ROOM_HEIGHT / 2) + (Game.HALLWAY_WIDTH * (Game.MAP_HEIGHT - 1)), 
                // y
                3,
                // z
                ((Game.ROOM_HEIGHT * Game.MAP_HEIGHT) / 2) + ((Game.HALLWAY_WIDTH * (Game.MAP_HEIGHT - 1)) / 2) - (Game.ROOM_HEIGHT / 2)
            );
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

                    GameObject tempFloor = Instantiate(floorPrefab, obj.transform);
                    tempFloor.name = "temp floor";

                    // Add and Set RoomData component
                    obj.AddComponent<RoomData>();
                    obj.GetComponent<RoomData>().SetData(i, x, z, roomPrefabs);

                    rooms[i] = obj;
                    Vector3 location = new Vector3((x * Game.ROOM_WIDTH) + (x * Game.HALLWAY_WIDTH), 0, (z * Game.ROOM_HEIGHT) + (z * Game.HALLWAY_WIDTH));
                    rooms[i].transform.localPosition = location;
                    Debug.Log(location);
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
            else
            {
                TurnOffWallTrigger();
                // DeactivateBoundaryWalls();
                DeleteAllCheckers();
                SpawnInternalRooms();
                BuildNavSurface();
                SpawnRoofs();
                SpawnPlayer();
            }
        }

        void BuildNavSurface()
        {
            rooms[0].GetComponent<RoomData>().BuildNavMesh();
        }

        void TurnOffWallTrigger()
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

            foreach (GameObject wall in walls)
            {
                wall.GetComponent<BoxCollider>().isTrigger = false;
            }
        }

        void SpawnRoofs()
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                GameObject roof = Instantiate(roofPrefab, rooms[i].transform);
                roof.transform.localPosition = new Vector3(0, 5.5f, 0);
                roof.transform.localScale = new Vector3(4, 1, 4);
            }
        }

        void SpawnPlayer()
        {
            player = Instantiate(playerPrefab, entityContainer.transform);
            player.transform.localPosition = new Vector3(0, 1, 0);
        }

        void DeleteAllCheckers()
        {
            GameObject[] checkers = GameObject.FindGameObjectsWithTag("Checker");

            foreach (GameObject checker in checkers)
            {
                Destroy(checker);
            }
        }

        void SpawnInternalRooms()
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                //GameObject roof = Instantiate(roofPrefab, rooms[i].transform);
                //roof.transform.localPosition = new Vector3(0, 5.5f, 0);
                //roof.transform.localScale = new Vector3(4, 1, 4);

                if (i == rooms.Length - 1)
                {
                    Instantiate(exitRoom, rooms[i].transform);
                }
            }
        }
    }
}
