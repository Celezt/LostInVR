using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaAxisMovement : MonoBehaviour
{

    private float RightHorizontalMovement, RightVerticalMovement;
    private Vector3 oldPosition, headsetRotation, newPosition;
    public float maxMoveSpeed = 5;
    private float moveSpeed;
    private bool blockedByObject = true;
    private int playerLayerMask = 1 << 16;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RightHorizontalMovement = Input.GetAxis("VRTK_Axis4_RightHorizontal");
        RightVerticalMovement = Input.GetAxis("VRTK_Axis5_RightVertical");
        moveSpeed = maxMoveSpeed;

        //if (RightVerticalMovement > 1)
        //{
        //    newpostion.position = transform.position + Camera.main.transform.forward * moveSpeed * Time.deltaTime;

        //}

        //oldPosition = this.transform.position;
        //headsetRotation = Camera.main.transform.forward;
        //headsetRotation.y = 0;
        //newPosition = oldPosition + (headsetRotation * moveSpeed);
        //Debug.Log("NewPos: " + newPosition + "...OldPos: " + oldPosition);
        //Debug.DrawLine(this.transform.position, newPosition, Color.blue);
        if (RightVerticalMovement < -0.05f)
        {
            oldPosition = this.transform.position;
            headsetRotation = Camera.main.transform.forward;
            headsetRotation.y = 0;

            newPosition = oldPosition + (headsetRotation * moveSpeed);
            blockedByObject = Physics.Linecast(oldPosition, newPosition, playerLayerMask);
            if (blockedByObject)
            {
                while (blockedByObject && (moveSpeed > 1))
                {
                    moveSpeed -= 0.5f;
                    if (moveSpeed < 1)
                    {
                        return;
                    }
                    newPosition = oldPosition + (headsetRotation * moveSpeed);
                    blockedByObject = Physics.Linecast(oldPosition, newPosition, playerLayerMask);
                    if (!blockedByObject)
                    {
                        this.transform.position += headsetRotation * moveSpeed * Time.deltaTime;
                        return;
                    }
                }
            }          
                      
            //Debug.Log("blocked? : " + blockedByObject);
            else 
            {
                this.transform.position += headsetRotation * moveSpeed * Time.deltaTime;
            }


        }
    }
}
