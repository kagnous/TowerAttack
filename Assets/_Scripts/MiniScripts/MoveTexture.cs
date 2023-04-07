using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTexture : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    
public float speed = 1.0f;
public Vector2 direction = Vector2.right;

void Update()
{
    float offset = Time.time * speed;
    GetComponent<Renderer>().material.mainTextureOffset = direction * offset;
}

