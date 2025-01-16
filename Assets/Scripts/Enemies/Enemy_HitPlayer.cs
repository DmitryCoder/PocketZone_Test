//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System.Collections;
using UnityEngine;

public class Enemy_HitPlayer : MonoBehaviour
{
    public EnemyObject enemy;
    private bool inCoolDown;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!inCoolDown && other.tag == "Player")
        {
            CharacterManager.instance.healthCurrent -= enemy.damageByHit;
            StartCoolDown();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inCoolDown && other.tag == "Player")
        {
            CharacterManager.instance.healthCurrent -= enemy.damageByHit;
            StartCoolDown();
        }
    }

    private void StartCoolDown()
    {
        inCoolDown = true;
        StartCoroutine(hold());
    }

    IEnumerator hold()
    {
        yield return new WaitForSeconds(1);
        inCoolDown = false;
    }
}
