using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_StoneDoor : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<ScaleManager>().level == 3)
        {
            coll.enabled = false;
            anim.SetTrigger("StoneDoorFall");
        }
    }

    public void destroyItem()
    {
        Destroy(gameObject);
    }
}
