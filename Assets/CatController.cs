using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    [Header("Cat")]
    public Animator CatAnim;
    public GameObject CatSprites;

    [Header("Game Manager")]
    public GameManager gm;
    

    [Header("Movement")]
    [SerializeField]
    float speed;

    [SerializeField]
    Rigidbody body;

    [SerializeField]
    float jumpHeight;

    [SerializeField]
    float extraGravity;

    [SerializeField, Range(0,1)]
    float airControl;

    bool isJumpingPressed;
    bool isJumping;
    public bool isFreakingOut;


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
        else
        {
            // check if a button is being pressed, if it is, move a bit in that direction
            if(Mathf.Abs(direction) > 0.01f)
            {
                float x = body.velocity.x;
                x += direction * Time.deltaTime * speed * 2.0f * airControl;
                x = Mathf.Clamp( x, -speed, speed );
                body.velocity = new Vector3( 0, body.velocity.y, 0 );
                body.AddForce( x * Vector3.right, ForceMode.VelocityChange);
                SetDirection( direction );
            }

        }

        // push cat down a bit more
        body.AddForce( extraGravity * Vector3.up, ForceMode.Acceleration );

        setFreakingOut(isFreakingOut);

//Jump
        if (!isJumping && isJumpingPressed)
        {
            body.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            isJumpingPressed = false;
            isJumping = true;

            Debug.Log("force is applied");
        }
    }

    public void setFreakingOut(bool freaking)
    {
        if (freaking)
        {
            // remove FreezeRotationZ
            body.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
            gm.SetFreakingOutTrue();
            StartCoroutine("onCatnip");

        }
        else
        {
            // set freezeRotation
            body.constraints |= RigidbodyConstraints.FreezeRotationZ;
            body.rotation = Quaternion.identity;
            gm.SetFreakingOutFalse();
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
        if ((otherThing.gameObject.tag == "floor") || (otherThing.gameObject.tag == "Shelf"))
        {
            isJumping = false;
            CatAnim.SetBool("isJumping", false);
        }

        if(otherThing.gameObject.tag == "Catnip")
        {
            isFreakingOut = true;
            gm.PlayFreakingOutSound();
            Destroy(otherThing.gameObject);
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
            gm.SetHidden();
            isFreakingOut = false;
            gm.SetFreakingOutFalse();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out");
        if (other.tag == "Hide")
        {
            CatAnim.SetBool("isHiding", false);
            gm.SetVisable();
            
        }
    }

    IEnumerator onCatnip()
    {
        yield return new WaitForSeconds(5);
        gm.SetFreakingOutFalse();
        isFreakingOut = false;

    }


}
