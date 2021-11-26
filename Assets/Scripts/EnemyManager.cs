using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Inst;

    private void Awake()
    {
        Inst = this;
    }

    public List<Enemy> enemys;

    public void UpdateStateTextAllEnemy()
    {
        for (int i = 0; i < this.enemys.Count; i++)
        {
            this.enemys[i].UpdateStateText();
        }
    }
}
