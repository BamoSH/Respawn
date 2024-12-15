using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Door : MonoBehaviour
{
    public GameObject aimProject;    
    public float slideSpeed = 2f;    
    private bool isOpening = false;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, aimProject.transform.position, slideSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, aimProject.transform.position) < 1f)
            {
                isOpening = false;
            }
        }
    }

    public void OpenDoor()
    {
        anim.SetTrigger("OpenDoor");
    }

    private void DoorMove()
    {
        isOpening = true;
    }
}
