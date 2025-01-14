//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_InventoryElement : MonoBehaviour
{
   public void ClickOnInventoryElement()
   {
      string elementId = this.name.Split("=")[1];

      InventoryManager.instance.ItemRemove_ShowConfirmationWindow(elementId);
   }
}
