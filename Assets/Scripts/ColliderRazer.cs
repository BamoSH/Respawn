using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRazer : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D component not found on the GameObject.");
        }
    }

    void Update()
    {
        AdjustColliderSize();
    }

    void AdjustColliderSize()
    {
        if (boxCollider != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                boxCollider.size = spriteRenderer.bounds.size;
            }
        }
    }
}
