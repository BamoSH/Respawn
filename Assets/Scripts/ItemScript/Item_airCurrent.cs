using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_airCurrent : MonoBehaviour
{
    public float updraftForce = 5f;  
    public bool initState = false;

    private Collider2D coll;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        coll.enabled = false;
    }

    private void Start()
    {
        if (initState)
        {
            coll.enabled = true;
            anim.SetTrigger("AirCurrent");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForce(new Vector2(0, updraftForce));
            }
        }
    }

    public void SwitchOn()
    {
        coll.enabled = true;
        anim.SetTrigger("AirCurrent");
    }
}
