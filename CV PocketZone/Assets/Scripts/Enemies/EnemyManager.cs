//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class EnemyManager : MonoBehaviour
{
    public GameObject prefabEnemy;
    private Tilemap tilemap;
    public static EnemyManager instance;
    

    private void Start()
    {
        if (instance == null) instance = this;
        tilemap = transform.GetChild(0).GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (DataManager.instance.spawnEnemies)
        {
            DataManager.instance.spawnEnemies = false;
            if(DataManager.session.enemies.Count>0) ReSpawnEnemies();
            else SpawnEnemies(3);
        }
    }

    public void ReSpawnEnemies()
    {
        foreach (int key in DataManager.session.enemies.Keys)
        {
            GameObject enemy = Instantiate(prefabEnemy,
                new Vector3(DataManager.session.enemies[key].positionX,
                    DataManager.session.enemies[key].positionY, 0f),
                Quaternion.identity);
            enemy.GetComponent<EnemyObject>().healthCurrent = DataManager.session.enemies[key].health;
            
        }
        DataManager.session.enemies.Clear();
    }

    public void SpawnEnemies(int enemyNum)
    {
        for (int x = 0; x < enemyNum; x++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        UnityEngine.Random.InitState(seed: DateTime.UtcNow.GetHashCode());
        Random rnd = new Random();
        int x = rnd.Next(tilemap.cellBounds.xMin+1, tilemap.cellBounds.xMax-1);
        int y = rnd.Next(tilemap.cellBounds.yMin+1, tilemap.cellBounds.yMax-1);
        Vector3Int localPlace = new Vector3Int(x, y, (int)tilemap.transform.position.y);
        Vector3 place = tilemap.CellToWorld(localPlace);
        Instantiate(prefabEnemy, place, quaternion.identity);
    }

    public class EnemyInRange
    {
        public GameObject enemyObject { get; set; }
        public SpriteRenderer hlCircle { get; set; }
    }
}
