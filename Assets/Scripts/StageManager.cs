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
                Vector3 enemy_position = enemy.hpbar.transform.position;
                float position_x = Mathf.Abs(enemy_position.x) - Mathf.Abs(enemy_spawn.position.x);
                float position_y = Mathf.Abs(enemy_position.y) - Mathf.Abs(enemy_spawn.position.y);
                enemy.transform.position += new Vector3(enemy_position.x > enemy_spawn.position.x ? -position_x : position_x,
                    enemy_position.y > enemy_spawn.position.y ? -position_y : position_y, i * -0.5f);
                EnemyManager.Inst.enemys.Add(enemy);
                enemy.hpbar.SetHP(monsterSO.monsters[i].hp);
                break;
            }
        }
    }
}
