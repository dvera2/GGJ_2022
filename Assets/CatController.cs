using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    Rigidbody body;

    [SerializeField]
    float jumpHeight;

    bool isJumpingPressed;
    bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float direction = Input.GetAxis("Horizontal");
 
        
        if (!isJumping)
        {
            body.velocity = new Vector3(0, body.velocity.y, 0);
            //Vector3 movement = (Vector3.right * Time.deltaTime * direction * speed);
            Vector3 movement = (Vector3.right * direction * speed);
            body.AddForce(movement, ForceMode.VelocityChange);
        }
        

        if (!isJumping && isJumpingPressed)
        {
            body.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            isJumpingPressed = false;
            isJumping = true;
            Debug.Log("force is applied");
        }
    }

    private void Update()
    {
        //if jumpinging isnt already pressed then read the input
        if (!isJumpingPressed && !isJumping)
        {
            isJumpingPressed = Input.GetKey(KeyCode.Space);          
            if (isJumpingPressed)
            {
                Debug.Log("jumped");
            }
            
        }
    }

    private void OnCollisionEnter(Collision otherThing)
    {
        isJumping = false;
    }
}
