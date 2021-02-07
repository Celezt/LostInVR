using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageRackSound : MonoBehaviour
{
    //private float soundWaitTimer = 0;   
    private float soundwaittime = 0.2f;
    private bool soundplayed = false;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (soundplayed)
        //{
        //    soundWaitTimer += Time.deltaTime;
        //    if (soundWaitTimer > soundwaittime)
        //    {
        //        soundplayed = false;
        //    }
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (!soundplayed)
        {
            AkSoundEngine.PostEvent("Footsteps", gameObject);
            soundplayed = true;
            coroutine = DelaySoundReplay();
            StartCoroutine(coroutine);
        }
    }

    IEnumerator DelaySoundReplay()
    {
        yield return new WaitForSeconds(soundwaittime);
        soundplayed = false;
    }
}
