using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoomData {
    /*房间的大小，每16*16作为一个单位区域，可以占有最多3*3区域面积*/
    public int sizeW, sizeH;
    /*三层Tilemap*/
    public int[] ground;
    public int[] groundDetails;
    public int[] onTheGround;
    /*桥的位置，每个单位区域都可以选择12个桥的位点，记录每个位点桥的深度，从1开始
    * 区域以行主序，位点从最左侧top出口开始顺时针编号，得到单元(i,j)的第m号出口索引为
    * (i*sizeW + j) * 12 + m */
    public int[]  bridgePositions;

    public RoomData(int w, int h) {
        sizeW = w;
        sizeH = h;
        ground = groundDetails = onTheGround = null;
        bridgePositions = null;
	}
}