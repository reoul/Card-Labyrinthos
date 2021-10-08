using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : MonoBehaviour
{
    public MapData mapdata;

    //void OnMouseDown()
    //{
    //    //MapManager.Inst.IconMouseDown();
    //}

    private void OnMouseUp()
    {
        MapData a = new MapData();
        MapManager.Inst.IconMouseUp(a);
    }
}
