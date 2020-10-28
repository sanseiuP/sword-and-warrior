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

    public int bridgeInBox_i(int i) {
        return i / 12 % sizeW;
	}

    public int bridgeInBox_j(int i) {
        return i / 12  / sizeW;
	}

    public int boxPositionToBridgeIndex(int i, int j, int no) {
        return (j * sizeW + i) * 12 + no;
	}

    public int boxIndexToBridgeIndex(int i, int no) {
        return (i / 3 * sizeW + i % 3) * 12 + no;
	}

    public int oppositeIndex(int i) {
        int result = -1;
        switch(i) {
            case 0: result = 8; break;
            case 1: result = 7; break;
            case 2: result = 6; break;
            case 3: result = 11; break;
            case 4: result = 10; break;
            case 5: result = 9; break;
            case 6: result = 2; break;
            case 7: result = 1; break;
            case 8: result = 0; break;
            case 9: result = 5; break;
            case 10: result = 4; break;
            case 11: result = 3; break;
        }
        return result;
	}
}