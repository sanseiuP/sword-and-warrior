using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager
{
    public Door[] doors;

    public void allDoorsOpen() {
        foreach(Door door in doors)
            door.setOpen();
	}

    public void allDoorsClose(){
        foreach(Door door in doors)
            door.setClose();
	}
}
