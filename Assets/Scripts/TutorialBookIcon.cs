using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialBookIconType { CARDS, SKILL }

public class TutorialBookIcon : MonoBehaviour
{
    bool onIcon = false;   //마우스가 필드 위에 있는지
    bool isGet = false;
    bool isFade = true;

    public TutorialBookIconType type;

    void OnMouseEnter()
    {
        onIcon = true;
    }
    void OnMouseExit()
    {
        onIcon = false;
    }

    private void OnMouseUp()
    {
        if (onIcon && !isGet && !isFade)
        {
            TutorialManager.Inst.Click(this);
        }
    }

    public IEnumerator SetLook()
    {
        while (true)
        {
            this.GetComponent<SpriteRenderer>().color += Color.black * 0.5f * Time.deltaTime;
            if (this.GetComponent<SpriteRenderer>().color.a >= 1)
                break;
            yield return new WaitForEndOfFrame();
        }
        isFade = false;
    }

    public void GetItem()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        isGet = true;
    }
}
