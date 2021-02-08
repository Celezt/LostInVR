using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGrabSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGrabSound()
    {
        AkSoundEngine.PostEvent("Item_slide", gameObject);
    }
}
