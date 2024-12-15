using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_TriggerFatDrip_Check : MonoBehaviour
{
    [SerializeField] private Item_FatDrip farDrip;

    private bool isActivated =false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            farDrip.PlayAnimation();
            isActivated = true;
        }
    }
}
