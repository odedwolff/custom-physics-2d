using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown("b"))
        {
            DropBall();
        }
        
    }

      private void DropBall(){
        Transform trans =  transform;
        //Debug.Log("trans.pos = " + trans.position);
        Instantiate(prefab, trans);
    }
}
