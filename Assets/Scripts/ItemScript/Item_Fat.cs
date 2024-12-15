using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Fat : MonoBehaviour
{
    [SerializeField] private int fatWeight = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<ScaleManager>().AddWeight(fatWeight);

            Destroy(gameObject);
        }
    }
}
