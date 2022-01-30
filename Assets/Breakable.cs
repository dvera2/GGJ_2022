using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameManager gm;
    public AudioClip sfx;
    public ParticleSystem shatter;
    public GameObject breakableImage;
    AudioSource audioSource;
    Collider breakCollider;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            shatter.Play();          
            audioSource.Play();
            gm.AddPoint();
            breakCollider.enabled = false;
            breakableImage.SetActive(false);
            Destroy(this.gameObject, 3);
        }
    }

}
