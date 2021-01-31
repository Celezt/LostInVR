using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonBehavior : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HandCollider")
        {
            this.gameObject.GetComponent<Renderer>().enabled = true;
            GameLoopManager.Instance.StartGame();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HandCollider")
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
