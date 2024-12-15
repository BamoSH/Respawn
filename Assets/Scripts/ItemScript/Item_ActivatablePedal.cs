using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ActivatablePedal : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    //private AudioManager am;
  //  public GameObject audioObj;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
       // am = audioObj.GetComponent<AudioManager>();
    }

    private void Start()
    {
        coll.enabled = false; // ¿ªÊ¼Ê±Òþ²Ø
    }

    public void ActivatePedal()
    {
        coll.enabled = true;
        anim.SetTrigger("Pedal");
        //am.PedalOut.Play();
    }
}
