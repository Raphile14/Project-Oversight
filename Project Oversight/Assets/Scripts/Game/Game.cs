using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.codingcatharsis.game
{
    public class Game : MonoBehaviour
    {
        // Player Default Stats
        public static float DEFAULT_PLAYER_SPEED = 100.0f;
        public static int DEFAULT_PLAYER_HEALTH = 10;
        public static float DEFAULT_DRAG = 6f;
        // TODO: Save sensitivity
        public static float DEFAULT_SENSX = 10f;
        public static float DEFAULT_SENSY = 10f;

        // Player Stamina
        public static float DEFAULT_MAX_STAMINA = 100f;
        public static float STAMINA_CONSUMPTION = 5f;
        public static float STAMINA_REGENERATION = 10f;

        // Player Current Stats
        public static int currentHealth;
        public static float currentStamina;

        // Map Dimensions
        public static int MAP_WIDTH = 3;
        public static int MAP_HEIGHT = 3;

        // Room Dimensions
        public static int ROOM_WIDTH = 40;
        public static int ROOM_HEIGHT = 40;
        public static float ROOM_SPAWN_WAIT = 0.5f;

        public static void SetDefaults()
        {
            currentHealth = DEFAULT_PLAYER_HEALTH;
            currentStamina = DEFAULT_MAX_STAMINA;
        }
    }
}
