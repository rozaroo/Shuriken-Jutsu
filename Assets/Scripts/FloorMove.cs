using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMove : MonoBehaviour
{
    private float speed = 1.25f;
    private float width = 6;
    public SpriteRenderer SpriteRenderer;
    private Vector2 startSize;
    
    void Start() 
    {
        startSize = new Vector2(SpriteRenderer.size.x, SpriteRenderer.size.y);
    }
    void Update() 
    {
        SpriteRenderer.size = new Vector2(SpriteRenderer.size.x + speed * Time.deltaTime, SpriteRenderer.size.y);
        if (SpriteRenderer.size.x > width) SpriteRenderer.size = startSize;
    }
}

