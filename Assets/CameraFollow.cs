using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform Cat;
    Vector3 currentPos;
    float catX;
    float catY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Cat )
        {
            currentPos = Cat.position;
            catX = currentPos.x;
            catY = currentPos.y;
            transform.position = new Vector3( catX, catY, transform.position.z );
        }
    }
}
