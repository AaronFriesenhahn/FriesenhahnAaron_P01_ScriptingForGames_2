using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierLogic : MonoBehaviour
{

    int x = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] EnemyArray;
        EnemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        //if the array is empty, destroy object
        if (EnemyArray.Length == 1)
        {
            Destroy(gameObject);
        }
    }
}
