using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FatDrip : MonoBehaviour
{
    public GameObject itemPrefab; 
    public Transform spawnPoint;

    private Animator anim;

    private void Awake()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    public void PlayAnimation()
    {
        anim.SetTrigger("Spit");
    }

    public void SpawnItem()
    {
        Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
    }
}
