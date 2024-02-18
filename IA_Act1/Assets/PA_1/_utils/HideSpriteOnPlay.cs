using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpriteOnPlay : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
