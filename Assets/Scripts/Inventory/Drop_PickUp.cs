//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using UnityEngine;

public class Drop_PickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (InventoryManager.instance.inventoryRoot.transform.childCount >= 4 && !InventoryManager.instance.activeItems.ContainsKey(name))
            {
                Debug.Log("Карманы переполнены");
                return;
            }

            InventoryManager.instance.Item_PickUp(name, 0);
            Destroy(gameObject);
        }
    }
}
