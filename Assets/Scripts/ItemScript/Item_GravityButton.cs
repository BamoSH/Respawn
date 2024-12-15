using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_GravityButton : MonoBehaviour
{
    public float sinkSpeed = 5f; 
    public float sinkDistance = 0.5f; 
    public float triggerLevel;

    private bool isActivated = false;
    private Vector3 initialPosition;
    private bool[] doorOpened;
    private bool[] fanOpened;

    //public GameObject[] aimItems;
    public List<GameObject> aimItems;

    void Start()
    {
        initialPosition = transform.position;
        doorOpened = new bool[aimItems.Count];
        fanOpened = new bool[aimItems.Count];
    }

    private void FixedUpdate()
    {
        if (isActivated)
        {
            // 计算下沉位置
            Vector3 targetPosition = initialPosition - new Vector3(0, sinkDistance, 0);
            // 向下移动按钮
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * sinkSpeed);
            // 确保按钮不超过目标位置
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isActivated = false; // 完成下沉
            }

            for (int i = 0; i < aimItems.Count; i++)
            {
                if (aimItems[i].TryGetComponent<Item_Door>(out Item_Door door) && !doorOpened[i])
                {
                    door.OpenDoor();
                    doorOpened[i] = true;
                }
                //Drive Fan
                if (aimItems[i].TryGetComponent<Item_Fan>(out Item_Fan fan) && !fanOpened[i])
                {
                    print("fan get");
                    fan.SwitchOn();
                    fan.GetComponentInChildren<Item_airCurrent>().SwitchOn();
                    fanOpened[i] = true;
                }
            }
            /*
            foreach(var aimItem in aimItems)
            {
                //Drive door
                if (aimItem.TryGetComponent<Item_Door>(out Item_Door door) && !doorOpened[])
                {
                    i++;
                    door.OpenDoor();
                    doorOpened[i] = true;
                }

                //Drive Fan
                if (aimItem.TryGetComponent<Item_airCurrent>(out Item_airCurrent airCurrent))
                {
                    airCurrent.SwitchOn();
                }
            }
            */
        }
    }

   

    private void OnTriggerStay2D(Collider2D collision)
    {
        print("Button Touch");
        //print("lock?" + collision.gameObject.GetComponent<PlayerController>().isLocked);
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<ScaleManager>().level >= triggerLevel && collision.gameObject.GetComponent<PlayerController>().isLocked)
        {
            print("Button Down");
            isActivated = true; // 玩家踩上按钮
        }
    }
}
