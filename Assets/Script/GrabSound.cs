using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSound : MonoBehaviour
{

    public AudioClip grab;
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GrabbableObject"))
        {
            audioSource.PlayOneShot(grab, 0.5f);
        }
    }
}
