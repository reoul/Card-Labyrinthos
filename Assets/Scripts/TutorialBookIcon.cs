using System.Collections;
using TMPro;
using UnityEngine;

public enum TutorialBookIconType { CARDS, SKILL, STARTBATTLE }

public class TutorialBookIcon : MonoBehaviour
{
    bool onIcon;   //마우스가 필드 위에 있는지
    bool isGet;
    bool isFade = true;

    public TutorialBookIconType type;

    void OnMouseEnter()
    {
        this.onIcon = true;
    }
    void OnMouseExit()
    {
        this.onIcon = false;
    }

    private void OnMouseUp()
    {
        if (this.onIcon && !this.isGet && !this.isFade)
        {
            TutorialManager.Inst.Click(this);
            this.isFade = true;
        }
    }

    public IEnumerator SetLook()
    {
        while (true)
        {
            this.GetComponent<SpriteRenderer>().color += Color.black * Time.deltaTime;
            if (this.transform.childCount > 0)
            {
                this.transform.GetChild(0).GetComponent<TMP_Text>().color += Color.black * Time.deltaTime;
            }
            if (this.GetComponent<SpriteRenderer>().color.a >= 1)
                break;
            yield return new WaitForEndOfFrame();
        }

        this.isFade = false;
    }

    public void GetItem()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        this.isGet = true;
    }
}
