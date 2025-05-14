using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private float velocity = 2;
    private Rigidbody2D rb2D;
    public AudioSource audioSource;
    public Sprite[] availableSprites;
    void Start() 
    {
        rb2D = GetComponent<Rigidbody2D>();
        Transform spriteChild = transform.Find("ShurikenSprite");
        if (spriteChild != null && ShurikenData.Instance != null) 
        {
            int index = ShurikenData.Instance.selectedShurikenIndex;
            SpriteRenderer sr = spriteChild.GetComponent<SpriteRenderer>();
            if (index >= 0 && index < availableSprites.Length) sr.sprite = availableSprites[index];
        }
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

