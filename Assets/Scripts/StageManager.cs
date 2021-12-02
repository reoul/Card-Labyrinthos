using System.Collections;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Inst;

    public MonsterSO monsterSO;

    public Transform enemy_spawn;

    public Sprite attackSprite;
    public Sprite healSprite;

    public TMP_Text debuffTMP;

    [SerializeField] private bool isTutorial;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        if (!isTutorial)
        {
            CreateStage();
        }
    }

    private void CreateStage()
    {
        if (MapManager.CurrentSceneName == "전투" || MapManager.CurrentSceneName == "보스")
        {
            SoundManager.Inst.Play(BACKGROUNDSOUND.Battle);
            foreach (Monster monster in monsterSO.monsters)
            {
                SoundManager.Inst.Play(MapManager.Inst.fieldData.monster_type == MONSTER_TYPE.Boss
                    ? BACKGROUNDSOUND.Boss
                    : BACKGROUNDSOUND.Battle);

                if (MapManager.Inst.fieldData.monster_type != monster.type)
                {
                    continue;
                }

                var enemy = Instantiate(monster.prefab, enemy_spawn.position, Quaternion.identity)
                    .GetComponent<Enemy>();
                if (MapManager.Inst.fieldData.monster_type == MONSTER_TYPE.Boss)
                {
                    enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                var enemyPosition = enemy.hpbar.transform.position;
                var position = enemyPosition - enemy_spawn.position;
                enemy.transform.position -= new Vector3(position.x, position.y, 0);

                EnemyManager.Inst.enemys.Add(enemy);
                enemy.hpbar.SetHp(monster.hp);
                enemy.attackDelay = monster.attackDelay;
                enemy.monster = monster;
                enemy.name = "Enemy";
                enemy.tag = "Enemy";
                break;
            }
        }

        debuffTMP.text = $"저주 : {DebuffManager.Inst.DebuffString}";
        DebuffManager.Inst.ApplyDebuff();
    }

    public IEnumerator CreateStageInTutorial()
    {
        foreach (Monster monster in monsterSO.monsters)
        {
            if (MapManager.Inst.fieldData.monster_type != monster.type)
            {
                continue;
            }

            var enemy = Instantiate(monster.prefab, enemy_spawn.position, Quaternion.identity)
                .GetComponent<Enemy>();
            var enemyPosition = enemy.hpbar.transform.position;
            var position = enemyPosition - enemy_spawn.position;
            enemy.transform.position -= new Vector3(position.x, position.y, 0);

            enemy.SetFixedWeaknessNum(MapManager.Inst.tutorialIndex == 1 ? 2 : -1);
            EnemyManager.Inst.enemys.Add(enemy);
            enemy.hpbar.SetHp(monster.hp);
            enemy.attackDelay = monster.attackDelay;
            enemy.monster = monster;
            enemy.name = "Enemy";
            enemy.tag = "Enemy";
            break;
        }

        yield return null;
    }
}
