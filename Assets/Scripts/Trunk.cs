using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    private float speed = 0.75f;
    void Update() 
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}

