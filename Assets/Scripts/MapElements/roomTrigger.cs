using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomTrigger : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player") )
            gameManager.notified("PlayerEnterRoom");

	}

}
