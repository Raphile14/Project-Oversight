using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace com.codingcatharsis.game
{
    public class Controller : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject floorPrefab;
        public GameObject roofPrefab;
        public GameObject hallwayFloorPrefab;
        public GameObject hallwayContainer;
        public GameObject[] roomPrefabs;
        public GameObject boundaryWallPrefab;
        public GameObject boundaryWallContainer;
        public GameObject roomContainer;
        public GameObject entityContainer;
        public GameObject enemyContainer;

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
                    // Vector3 location = new Vector3((x * Game.ROOM_WIDTH), 0, (z * Game.ROOM_HEIGHT));
                    rooms[i].transform.localPosition = location;
                    Debug.Log(location);
                    i += 1;
                }
            }
        }

        //public bool CheckPath()
        //{
        //    if (currentRoom == rooms.Length) return true;
        //    pathChecker.enabled = false;
        //    Vector3 starting = new Vector3((Game.MAP_WIDTH / 2) * Game.ROOM_WIDTH, 1, rooms[currentRoom].GetComponent<RoomData>().getzCoord() * Game.ROOM_HEIGHT);
        //    // Vector3 starting = new Vector3(0, 1, 0);
        //    //if (currentRoom > 0)
        //    //{
        //    //    Vector3 newStart = new Vector3(rooms[currentRoom - 1].GetComponent<RoomData>().getxCoord() * Game.ROOM_WIDTH, 1, rooms[currentRoom - 1].GetComponent<RoomData>().getzCoord() * Game.ROOM_HEIGHT);
        //    //    Debug.Log(newStart);
        //    //    starting = newStart;
        //    //}
        //    testCube.transform.position = starting;
        //    pathChecker.enabled = true;
        //    Debug.Log("Checking Path");
        //    //int completePaths = 0;
        //    //for (int i = 0; i < rooms.Length; i++)
        //    //{
        //    //    if (currentRoom == i) continue;
        //    //    bool status = false;
        //    //    NavMeshPath path = new NavMeshPath();
        //    //    Vector3 target = new Vector3(rooms[i].GetComponent<RoomData>().getxCoord() * Game.ROOM_WIDTH, 1, rooms[i].GetComponent<RoomData>().getzCoord() * Game.ROOM_HEIGHT);
        //    //    //NavMesh.CalculatePath(starting, target, NavMesh.AllAreas, path);
        //    //    pathChecker.CalculatePath(target, path);
        //    //    if (path.status == NavMeshPathStatus.PathComplete) // 
        //    //    {
        //    //        status = true;
        //    //        //completePaths += 1;
        //    //    }
        //    //    //else if (path.status == NavMeshPathStatus.PathPartial)
        //    //    //{
        //    //    //    int maxTries = Game.MAP_WIDTH * Game.MAP_HEIGHT;
        //    //    //    Debug.Log("last path length: " + path.corners.Length);
        //    //    //    Debug.Log("last path: " + path.corners[path.corners.Length - 1]);
        //    //    //    Time.timeScale = 0;
        //    //    //}
        //    //    Debug.Log("Path Status: " + path.status);
        //    //    Debug.Log("i: " + i + " status: " + status);
        //    //    if (!status)
        //    //    {
        //    //        return false;
        //    //    }
        //    //}
        //    bool status = false;
        //    NavMeshPath path = new NavMeshPath();
        //    Debug.Log("Current Room: " + currentRoom);
        //    Vector3 target = new Vector3(rooms[currentRoom].GetComponent<RoomData>().getxCoord() * Game.ROOM_WIDTH, 1, rooms[currentRoom].GetComponent<RoomData>().getzCoord() * Game.ROOM_HEIGHT);
        //    Debug.Log("Current Target: " + target);
        //    //NavMesh.CalculatePath(starting, target, NavMesh.AllAreas, path);
        //    pathChecker.CalculatePath(target, path);
        //    if (path.status == NavMeshPathStatus.PathComplete)
        //    {
        //        status = true;
        //    }
        //    Debug.Log("Corners: " + path.corners.Length);
        //    Debug.Log("Path Status: " + path.status);
        //    Debug.Log("i: " + currentRoom + " status: " + status);
        //    if (!status)
        //    {
        //        return false;
        //    }
        //    //if (completePaths < 3)
        //    //{
        //    //    return false;
        //    //}
        //    return true;
        //}

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
            Debug.Log("Spawned Roofs");
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
    }
}
