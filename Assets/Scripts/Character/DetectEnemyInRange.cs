//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemyInRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Enemy") return;
        
            if (!CharacterController.instance.enemiesInRange.ContainsKey(other.gameObject.GetInstanceID()))
            {
                CharacterController.instance.enemiesInRange.Add(other.gameObject.GetInstanceID(), new EnemyManager.EnemyInRange()
                {
                    enemyObject = other.gameObject,
                    hlCircle = other.transform.Find("rangeAttack").GetComponent<SpriteRenderer>()
                });
            }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Enemy") return;
        if (CharacterController.instance.enemiesInRange.ContainsKey(other.gameObject.GetInstanceID()))
        {
            CharacterController.instance.enemiesInRange.Remove(other.gameObject.GetInstanceID());
        }

        other.transform.Find("rangeAttack").GetComponent<SpriteRenderer>().enabled = false;
    }
    
    
}
