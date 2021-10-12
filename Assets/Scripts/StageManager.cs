using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Inst;

    public MonsterSO monsterSO;

    public Transform player_spawn;
    public Transform enemy_spawn;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        CreateStage();
    }

    public void CreateStage()
    {
        for (int i = 0; i < monsterSO.monsters.Length; i++)
        {
            if (MapManager.Inst.fieldData.monster_type == monsterSO.monsters[i].type)
            {
                Enemy enemy = Instantiate(monsterSO.monsters[i].prefab, enemy_spawn.position, Quaternion.identity).GetComponent<Enemy>();
                EnemyManager.Inst.enemys.Add(enemy);
                enemy.hpbar.SetHP(monsterSO.monsters[i].hp);
                break;
            }
        }
    }
}
