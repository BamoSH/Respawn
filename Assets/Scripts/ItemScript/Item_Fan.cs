using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Fan : MonoBehaviour
{
    public bool initState = false;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if(initState)
        {
            anim.SetTrigger("Fan");
        }
    }

    public void SwitchOn()
    {
        anim.SetTrigger("Fan");
    }
}
