//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StartAttack : MonoBehaviour
{
    [SerializeField]private EnemyObject enemy;
    private void OnTriggerEnter2D(Collider2D other)
      {
          if (other.tag == "Player") enemy.isAttacking = true;
      }
}
