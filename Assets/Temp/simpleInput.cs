﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Actor player;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A)) {
            player.setMove(3.1415926f);
		}
        if(Input.GetKeyUp(KeyCode.A)) {
            player.setStand();
		}
        if(Input.GetKey(KeyCode.D)) {
            player.setMove(0);
		}
        if(Input.GetKeyUp(KeyCode.D)) {
            player.setStand();
		}
        if(Input.GetKey(KeyCode.W)) {
            player.setMove(3.1415926f/2);
		}
        if(Input.GetKeyUp(KeyCode.W)) {
            player.setStand();
		}
        if(Input.GetKey(KeyCode.S)) {
            player.setMove(-3.1415926f/2);
		}
        if(Input.GetKeyUp(KeyCode.S)) {
            player.setStand();
		}
    }
}
