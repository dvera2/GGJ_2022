using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform Cat;
    Vector3 currentPos;
    float catX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = Cat.position;
        catX = currentPos.x;
        transform.position = new Vector3(catX, transform.position.y, transform.position.z);
    }
}
