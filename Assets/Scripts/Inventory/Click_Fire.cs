//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using UnityEngine;

public class Click_Fire : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FirePressed()
    {

        if(!InventoryManager.instance.CurrentAmmoFired(1)) return; //нет патронов
        audioSource.Play();
        CharacterManager.instance.ShootNearestEnemy(1);
        InventoryManager.instance.InventoryValues_RefreshUI();
    }
}
