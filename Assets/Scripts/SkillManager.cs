using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Inst = null;
    bool isOpen = false;

    public List<SkillBookCard> skillBookCards;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Init()
    {
        this.transform.position = new Vector3(0, 0, -2);
    }

    public void Open()      //스킬창 여는 것
    {
        if (isOpen)
        {
            Close();
            return;
        }
        GameManager.Inst.CloseAllUI();
        isOpen = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Close()      //스킬창 여는 것
    {
        transform.GetChild(0).gameObject.SetActive(false);
        isOpen = false;
    }
}
