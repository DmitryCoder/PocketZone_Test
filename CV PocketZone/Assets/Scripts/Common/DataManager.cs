//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public static DataSession session = new DataSession();
    public bool spawnEnemies;
    public bool spawnPlayer;
    public bool respawnInventory;
    public bool loadInventory;
    private void Awake()
    {
        if (instance == null) instance = this;
        Application.quitting += QuitActions;
        string path = Path.Combine(Application.persistentDataPath, "session.data");
        if (File.Exists(path))
        {
            session = JsonConvert.DeserializeObject<DataSession>(File.ReadAllText(path));
            respawnInventory = true;
        }
        spawnEnemies = true;
        spawnPlayer = true;
        
        loadInventory = true;
    }

    public void QuitActions()
    {
        string path = Path.Combine(Application.persistentDataPath, "session.data");
        string data = JsonConvert.SerializeObject(session);
        
        File.WriteAllText(path, data);
    }

    public void WritePlayerState(Vector2 pos, int health)
    {
        session.playerPositionX = pos.x;
        session.playerPositionY = pos.y;
        session.playerHealth = health;
    }

    public void WriteEnemyState(int enemyId, Vector2 pos, int health)
    {
        if (health <= 0)
        {
            RemoveEnemyState(enemyId);
            return;
        }
        if (!session.enemies.ContainsKey(enemyId))
            session.enemies.Add(enemyId, new EnemyData()
                {
                    positionX = pos.x,
                    positionY = pos.y,
                    health = health
                }
            );
        else
        {
            session.enemies[enemyId].positionX = pos.x;
            session.enemies[enemyId].positionY = pos.y;
            session.enemies[enemyId].health = health;
        }
    }

    public void RemoveEnemyState(int enemyId)
    {
        if (session.enemies.ContainsKey(enemyId)) session.enemies.Remove(enemyId);
    }

    public void UpdateItemInInventory(string itemId, int amount)
    {
        session.inventory[itemId] = amount;
    }

    public void RemoveItemFromInventory(string itemId)
    {
        if (session.inventory.ContainsKey(itemId)) session.inventory.Remove(itemId);
    }

    [Serializable] public class DataSession
    {
        public DataSession()
        {
            enemies = new Dictionary<int, EnemyData>();
            inventory = new Dictionary<string, int>();
            playerPositionX = 0;
            playerPositionY = 0;
            playerHealth = 10;
        }

        public Dictionary<int, EnemyData> enemies;
        public Dictionary<string, int> inventory;
        public float playerPositionX;
        public float playerPositionY;
        public int playerHealth;
    }

    public class EnemyData
    {
        public float positionX;
        public float positionY;
        public int health;
    }
    
}
