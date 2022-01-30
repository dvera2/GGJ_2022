using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameManager gm;
    public AudioClip sfx;
    public ParticleSystem shatter;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sfx;
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
          //  Destroy(this.gameObject);
        }
    }
}
