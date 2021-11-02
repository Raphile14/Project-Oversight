using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.codingcatharsis.game
{
    public class Controller : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject[] roomPrefabs;
        public GameObject roomContainer;
        public GameObject entityContainer;        
        public GameObject enemyContainer;

        // Instantiated Objects
        GameObject player;
        GameObject[,] rooms;

        void Start()
        {
            SpawnRooms();
            SpawnPlayer();
        }

        void SpawnRooms()
        {
            rooms = new GameObject[Game.MAP_WIDTH, Game.MAP_HEIGHT];

            for (int z = 0; z < Game.MAP_WIDTH; z++)
            {
                for (int x = 0; x < Game.MAP_HEIGHT; x++)
                {
                    rooms[x, z] = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], roomContainer.transform);
                    rooms[x, z].transform.localPosition = new Vector3(x * Game.ROOM_WIDTH, 0, z * Game.ROOM_HEIGHT);
                }
            }
        }

        void SpawnPlayer()
        {
            player = Instantiate(playerPrefab, entityContainer.transform);
        }
    }
}
