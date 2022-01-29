using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    [Header("Movement")]
    public Animator CatAnim;
    public GameObject CatSprites;
    

    [Header("Movement")]
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
 
 //Walk       
        if (!isJumping)
        {
            body.velocity = new Vector3(0, body.velocity.y, 0);
            //Vector3 movement = (Vector3.right * Time.deltaTime * direction * speed);
            Vector3 movement = (Vector3.right * direction * speed);
            body.AddForce(movement, ForceMode.VelocityChange);
            SetDirection(direction);

        }



        
//Jump
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
                CatAnim.SetBool("isJumping", true);
                Debug.Log("jumped");
            }
            
        }
    }

    private void OnCollisionEnter(Collision otherThing)
    {
        if (otherThing.gameObject.tag == "floor")
        {
            isJumping = false;
            CatAnim.SetBool("isJumping", false);
        }
        
    }

    void SetDirection(float dir)
    {
        if(dir == 0)
        {
            return;
        }
        Vector3 CatScale = CatSprites.transform.localScale;
        CatScale.x = Mathf.Abs(CatScale.x) * Mathf.Sign(dir);
        CatSprites.transform.localScale = CatScale;
        CatAnim.SetFloat("Walking", Mathf.Abs(dir));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hiding");
        if (other.tag == "Hide")
        {
            CatAnim.SetBool("isHiding", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out");
        if (other.tag == "Hide")
        {
            CatAnim.SetBool("isHiding", false);
        }
    }
}
