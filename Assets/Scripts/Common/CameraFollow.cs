//тестовое задание. Дмитрий Малахаев, nazgul7@yandex.ru
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    void Update()
    {
        if(target!=null)transform.position =new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
