//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObject : MonoBehaviour
{
    public int healthMax = 10;
    public int healthCurrent = 10;
    public RectTransform healthbar;
    public bool isAttacking;
    [SerializeField] private float speed = 1f;
    private Rigidbody2D rb;
    private GameObject player;
    public int damageByHit = 1;
    
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = CharacterController.instance.gameObject;
    }

    private void FixedUpdate()
    {
        float scale = (float)healthCurrent / healthMax;
        healthbar.localScale = new Vector3(scale, healthbar.localScale.y, healthbar.localScale.z);
        DataManager.instance.WriteEnemyState(gameObject.GetInstanceID(), rb.position, healthCurrent);
        
        if (isAttacking) MoveToPlayer();
        ShouldIDie();
        
    }

    private void MoveToPlayer()
    {
        if(player!=null)transform.position=Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
    }

    private void ShouldIDie()
    {
        //умираем если надо
        if (healthCurrent > 0) return;
        Vector3 coords = gameObject.transform.position;
        int myId = gameObject.GetInstanceID();
        if (CharacterController.instance.enemiesInRange.ContainsKey(myId))
            CharacterController.instance.enemiesInRange.Remove(myId);

        if (CharacterController.instance.nearestEnemyId == myId) CharacterController.instance.nearestEnemyId = 0;
        
        DataManager.instance.RemoveEnemyState(gameObject.GetInstanceID());
        Destroy(gameObject);
        
        //спавним дроп
        InventoryManager.instance.SpawnDrop(coords);
    }
}
