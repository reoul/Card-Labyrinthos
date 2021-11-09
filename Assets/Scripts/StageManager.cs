using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Inst;

    public MonsterSO monsterSO;

    public Transform player_spawn;
    public Transform enemy_spawn;

    public Sprite attackSprite;
    public Sprite healSprite;

    public TMP_Text debuffTMP;

    [SerializeField] bool isTutorial = false;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        if (!isTutorial)
            CreateStage();
    }

    public void CreateStage()
    {
        if (MapManager.Inst.CurrentSceneName == "전투" || MapManager.Inst.CurrentSceneName == "보스")
        {
            SoundManager.Inst.Play(BACKGROUNDSOUND.BATTLE);
            for (int i = 0; i < monsterSO.monsters.Length; i++)
            {
                if (MapManager.Inst.fieldData.monster_type == MONSTER_TYPE.BOSS)
                    SoundManager.Inst.Play(BACKGROUNDSOUND.BOSS);
                else
                    SoundManager.Inst.Play(BACKGROUNDSOUND.BATTLE);
                if (MapManager.Inst.fieldData.monster_type == monsterSO.monsters[i].type)
                {
                    Enemy enemy = Instantiate(monsterSO.monsters[i].prefab, enemy_spawn.position, Quaternion.identity).GetComponent<Enemy>();
                    if (MapManager.Inst.fieldData.monster_type == MONSTER_TYPE.BOSS)
                    {
                        enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    Vector3 enemy_position = enemy.hpbar.transform.position;
                    float position_x = enemy_position.x - enemy_spawn.position.x;
                    float position_y = enemy_position.y - enemy_spawn.position.y;
                    enemy.transform.position -= new Vector3(position_x, position_y, 0);

                    EnemyManager.Inst.enemys.Add(enemy);
                    enemy.hpbar.SetHP(monsterSO.monsters[i].hp);
                    enemy.attackDelay = monsterSO.monsters[i].attackDelay;
                    //enemy.hpbar.hp = 5;
                    enemy.monster = monsterSO.monsters[i];
                    enemy.name = "Enemy";
                    enemy.tag = "Enemy";
                    break;
                }
            }
        }
        debuffTMP.text = string.Format($"저주 : {DebuffManager.Inst.DebuffString}");
        DebuffManager.Inst.ApplyDebuff();
    }

    public IEnumerator CreateStageInTutorial()
    {
        for (int i = 0; i < monsterSO.monsters.Length; i++)
        {
            if (MapManager.Inst.fieldData.monster_type == monsterSO.monsters[i].type)
            {
                Enemy enemy = Instantiate(monsterSO.monsters[i].prefab, enemy_spawn.position, Quaternion.identity).GetComponent<Enemy>();
                Vector3 enemy_position = enemy.hpbar.transform.position;
                float position_x = enemy_position.x - enemy_spawn.position.x;
                float position_y = enemy_position.y - enemy_spawn.position.y;
                enemy.transform.position -= new Vector3(position_x, position_y, 0);
                enemy.SetFixedWeaknessNum(MapManager.Inst.tutorialIndex == 1 ? 2 : -1);
                EnemyManager.Inst.enemys.Add(enemy);
                enemy.hpbar.SetHP(monsterSO.monsters[i].hp);
                enemy.attackDelay = monsterSO.monsters[i].attackDelay;
                //enemy.hpbar.hp = 5;
                enemy.monster = monsterSO.monsters[i];
                enemy.name = "Enemy123";
                enemy.tag = "Enemy";
                break;
            }
        }
        yield return null;
        //debuffTMP.text = string.Format($"저주 : {DebuffManager.Inst.DebuffString}");
        //DebuffManager.Inst.ApplyDebuff();
    }
}
