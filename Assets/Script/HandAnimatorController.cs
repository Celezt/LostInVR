using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimatorController : MonoBehaviour
{
    public Animator HandAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseHand()
    {
        HandAnimator.SetInteger("Pose", 1);
    }
    public void OpenHand()
    {
        HandAnimator.SetInteger("Pose", 0);
    }
}
