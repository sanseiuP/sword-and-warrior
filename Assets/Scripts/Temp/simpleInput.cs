using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleInput : MonoBehaviour
{
    /*
    以下内容保留



    public Actor player=new Actor();
    Attack attack;
    static Command command=new Command();

    void Awake()//需要在这里对player、attack和command实例化，否则update（）会提示空指针
    {
        
        

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        if (player!=null&&command!=null) command.move(player);
        
    }
    */
    // Start is called before the first frame update
    /*

    void Start()
    {
        
    }

    public Warrior player;
    Attack attack;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
            player.setWave();
        int move_direction=InputManager.getInstance().getMoveInput();
        switch (move_direction)
        {
            case 0b0000:
            case 0b0101:
            case 0b1010:
            case 0b1111:
                break;
            case 0b0001:
            case 0b1011:
                player.SetMove(0,player.GetComponent<Rigidbody2D>(), player.GetComponent<Animator>(),player.GetComponent<AudioSource>());
                break;
            case 0b0010:
            case 0b0111:
                player.SetMove(-3.1415926f/2, player.GetComponent<Rigidbody2D>(), player.GetComponent<Animator>(), player.GetComponent<AudioSource>());
                break;
            case 0b0011:
                player.SetMove(-3.1415926f/4, player.GetComponent<Rigidbody2D>(), player.GetComponent<Animator>(), player.GetComponent<AudioSource>());
                break;
            case 0b0100:
            case 0b1110:
                player.SetMove(-3.1415926f, player.GetComponent<Rigidbody2D>(), player.GetComponent<Animator>(), player.GetComponent<AudioSource>());
                break;
            case 0b0110:
                player.SetMove(-3.1415926f*3/4, player.GetComponent<Rigidbody2D>(), player.GetComponent<Animator>(), player.GetComponent<AudioSource>());
                break;
            case 0b1000:
            case 0b1101:
                player.SetMove(3.1415926f/2, player.GetComponent<Rigidbody2D>(), player.GetComponent<Animator>(), player.GetComponent<AudioSource>());
                break;
            case 0b1001:
                player.SetMove(3.1415926f/4, player.GetComponent<Rigidbody2D>(), player.GetComponent<Animator>(), player.GetComponent<AudioSource>());
                break;
            case 0b1100:
                player.SetMove(3.1415926f*3/4, player.GetComponent<Rigidbody2D>(), player.GetComponent<Animator>(), player.GetComponent<AudioSource>());
                break;            
            default:
                break;
        }
    }
    */


}
