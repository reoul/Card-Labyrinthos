using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : MonoBehaviour
{
    void OnMouseDown()
    {
        MapManager.Inst.IconMouseDown();
    }

    private void OnMouseUp()
    {
        MapManager.Inst.IconMouseUp();
    }
}
