using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public AudioClip sfx;
    public ParticleSystem shatter;
    public GameObject breakableImage;
    AudioSource audioSource;
    Collider breakCollider;
    public Rigidbody body;

    [Header("Value")]
    public float ItemValue;

    bool broken;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sfx;

        breakCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter( Collider other )
    {
        if( other.gameObject.GetComponent<CatController>() )
        {
            body.isKinematic = false;
            body.AddForceAtPosition( other.attachedRigidbody.velocity * 0.5f, body.position );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            BreakThing();

        }
    }

    private void BreakThing()
    {
        if( !broken )
        {
            broken = true;
            shatter.Play();
            audioSource.Play();
            GameManager.TriggerAddPoint( ItemValue );
            breakCollider.enabled = false;
            breakableImage.SetActive( false );
            Destroy( this.gameObject, 3 );
        }
    }

    float timeTouched = 0;
    private void OnCollisionStay( Collision collision )
    {
        if( collision.gameObject.CompareTag("Player") )
        {
            timeTouched += Time.fixedDeltaTime;

            if( timeTouched > 1.0f )
                BreakThing();
        }
    }



}
