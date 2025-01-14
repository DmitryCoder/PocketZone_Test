//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_RemoveNo : MonoBehaviour
{
    public void RejectRemoval()
    {
        Destroy(transform.parent.gameObject);
    }
}
