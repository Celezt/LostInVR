﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper : MonoBehaviour
{
    public int MaxGameTime = 120;
    // Start is called before the first frame update
    void Start()
    {
        GameLoopManager.Instance.MaxGameTime = MaxGameTime;
        GameLoopManager.Instance.ResetScene();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartGame();                
        }
    }

    public void StartGame()
    {
        GameLoopManager.Instance.StartGame();
    }
}