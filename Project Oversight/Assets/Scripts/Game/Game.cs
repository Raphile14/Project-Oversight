using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.codingcatharsis.game
{
    public class Game : MonoBehaviour
    {
        // Player Default Stats
        public static float DEFAULT_PLAYER_SPEED = 30.0f;
        public static float DEFAULT_PLAYER_SPRINTSPEED = 80.0f;
        public static int DEFAULT_PLAYER_HEALTH = 10;
        public static float DEFAULT_DRAG = 6f;
        // TODO: Save sensitivity
        public static float DEFAULT_SENSX = 15f;
        public static float DEFAULT_SENSY = 15f;

        // Player Stamina
        public static float DEFAULT_MAX_STAMINA = 100f;
        public static float STAMINA_CONSUMPTION = 10f;
        public static float STAMINA_REGENERATION = 15f;

        // Player Current Stats
        public static int currentHealth;
        public static float currentStamina;

        // Map Dimensions
        public static int MAP_WIDTH = 5;
        public static int MAP_HEIGHT = 5;

        // Room Dimensions
        public static int ROOM_WIDTH = 40;
        public static int ROOM_HEIGHT = 40;
        public static float ROOM_SPAWN_WAIT = 0.1f;

        // Hallway Dimensions
        public static int HALLWAY_HEIGHT = 40;
        public static int HALLWAY_WIDTH = 20;

        // Seed Data
        public static int seed = -1;

        // Objectives
        public static int TOTAL_TASKS = 10;

        public static void SetDifficulty(int value)
        {
            switch (value)
            {
                // Easy
                default:
                    MAP_WIDTH = 3;
                    MAP_HEIGHT = 3;
                    TOTAL_TASKS = 10;
                    break;
                // Normal
                case 1:
                    MAP_WIDTH = 5;
                    MAP_HEIGHT = 5;
                    TOTAL_TASKS = 20;
                    break;
                // Hard
                case 2:
                    MAP_WIDTH = 7;
                    MAP_HEIGHT = 7;
                    TOTAL_TASKS = 30;
                    break;
            }
            Debug.Log("Width: " + MAP_WIDTH + " | Height: " + MAP_HEIGHT);
        }
        public static void SetSeed(int value)
        {
            seed = value;
        }

        public static void InitRandomGenerator()
        {
            if (seed == -1)
            {
                seed = Random.Range(0, 1000000);
                Debug.Log("Random Seed: " + seed);
            }
            else
            {
                Debug.Log("Selected Seed: " + seed);
            }            
            Random.InitState(seed);
        }

        public static void SetDefaults()
        {
            currentHealth = DEFAULT_PLAYER_HEALTH;
            currentStamina = DEFAULT_MAX_STAMINA;
        }
    }
}
