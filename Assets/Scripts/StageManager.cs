using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Inst;

    public Transform player_spawn;
    public Transform enemy_spawn;

    private void Awake()
    {
        Inst = this;
    }

    public void CreateStage()
    {

    }
}
