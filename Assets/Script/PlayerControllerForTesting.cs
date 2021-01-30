using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerForTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        // Gets inputs for increasing score or time if the game loop is active
        if (GameLoopManager.Instance.gameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameLoopManager.Instance.IncreaseScore(15);

            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                GameLoopManager.Instance.IncreaseTimeRemaining(5);
            }
        }        
      
        else if (Input.GetKeyDown(KeyCode.R))
        {
            GameLoopManager.Instance.ResetScene();
        }
    }
}
