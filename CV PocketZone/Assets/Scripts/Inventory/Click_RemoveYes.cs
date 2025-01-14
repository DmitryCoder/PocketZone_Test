//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_RemoveYes : MonoBehaviour
{
    public void ApproveRemoval()
    {
        
        InventoryManager.instance.ItemRemove_Remove();
        Destroy(transform.parent.gameObject);
    }
}
