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
        foreach (Enemy enemy in enemys)
        {
            enemy.UpdateStateText();
        }
    }
}
