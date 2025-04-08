using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private float velocity = 2;
    private Rigidbody2D rb2D;
    public AudioSource audioSource;
    void Start() 
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Update() 
    {
        if (Input.touchCount > 0) 
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) rb2D.velocity = Vector2.up * velocity;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        FindAnyObjectByType<GameManager>().GameOver();
        audioSource.Play();
    }
}

