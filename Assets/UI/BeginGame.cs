using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginGame : MonoBehaviour
{
    public void beginGame()
    {
        SceneManager.LoadScene("RoomTest");
    }
}
