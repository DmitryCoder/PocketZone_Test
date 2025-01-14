//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public VariableJoystick variableJoystick;
    private Rigidbody2D rb;
    public int speed;
    public int healthMax = 10;
    public int healthCurrent = 10;
    private Vector2 movement;
    public RectTransform healthbar;
    public static CharacterController instance;
    [NonSerialized]public int nearestEnemyId = 0;
    public Dictionary<int, EnemyManager.EnemyInRange> enemiesInRange = new Dictionary<int, EnemyManager.EnemyInRange>();
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (instance == null) instance = this;
    }

    private void LoadPrevData()
    {
        transform.position = new Vector3(DataManager.session.playerPositionX,
            DataManager.session.playerPositionY, 0);
        healthCurrent = DataManager.session.playerHealth;
    }

    void Update()
    {
        
        if (DataManager.instance.spawnPlayer && DataManager.session.playerHealth>0)
        {
            DataManager.instance.spawnPlayer = false;
            LoadPrevData();
        } else movement = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical).normalized*speed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position+movement*Time.fixedDeltaTime);
        HightLightNearestEnemy();
        
        float scale = (float)healthCurrent / healthMax;
        healthbar.localScale = new Vector3(scale, healthbar.localScale.y, healthbar.localScale.z);
        ShouldIDie();
        if(!DataManager.instance.spawnPlayer) DataManager.instance.WritePlayerState(rb.position, healthCurrent);
    }

    private void ShouldIDie()
    {
        if (healthCurrent > 0) return;
        
        Destroy(gameObject);
    }

    private void HightLightNearestEnemy()
    {
        float minRange = 1000f;
        int prevKey = -1;
        foreach (int key in enemiesInRange.Keys)
        {
            float distance = Vector3.Distance(transform.position, enemiesInRange[key].enemyObject.transform.position);
            if (minRange > distance)
            {
                if (prevKey != -1)
                {
                    HightLightEnemy(enemiesInRange[prevKey].enemyObject, false);
                }

                nearestEnemyId = key;
                HightLightEnemy(enemiesInRange[key].enemyObject, true);
                minRange = distance;
            } else HightLightEnemy(enemiesInRange[key].enemyObject, false);

            prevKey = key;
        }
    }

    public void ShootNearestEnemy(int damage)
    {
        if (nearestEnemyId == 0) return;
        enemiesInRange[nearestEnemyId].enemyObject.GetComponent<EnemyObject>().healthCurrent -= damage;
    }

    private void HightLightEnemy(GameObject enemy, bool active)
    {
        GameObject hl = enemy.transform.Find("rangeAttack").gameObject;
        if (active) hl.GetComponent<SpriteRenderer>().enabled = true;
        else hl.GetComponent<SpriteRenderer>().enabled = false;
    }
}
