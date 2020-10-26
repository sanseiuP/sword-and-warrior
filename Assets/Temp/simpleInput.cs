using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleInput : MonoBehaviour
{
    
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
}
