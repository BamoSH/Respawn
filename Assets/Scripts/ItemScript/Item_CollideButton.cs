using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_CollideButton : MonoBehaviour
{
    public Item_ActivatablePedal pedal;

    private Animator anim;
    private bool used;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("collide");
        if (collision.CompareTag("TransferFat") && !used)
        {
            print("Pedal!");
            anim.SetTrigger("CollideButton");

            pedal.ActivatePedal();
            used = true;
        }
    }
}
