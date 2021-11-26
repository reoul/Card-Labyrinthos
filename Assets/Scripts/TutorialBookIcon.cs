using System.Collections;
using TMPro;
using UnityEngine;

public enum TutorialBookIconType { CARDS, SKILL, STARTBATTLE }

public class TutorialBookIcon : MonoBehaviour
{
    private bool onIcon;   //마우스가 필드 위에 있는지
    private bool isGet;
    private bool isFade = true;

    public TutorialBookIconType type;

    private void OnMouseEnter()
    {
        onIcon = true;
    }

    private void OnMouseExit()
    {
        onIcon = false;
    }

    private void OnMouseUp()
    {
        if (onIcon && !isGet && !isFade)
        {
            TutorialManager.Inst.Click(this);
            isFade = true;
        }
    }

    public IEnumerator SetLook()
    {
        while (true)
        {
            GetComponent<SpriteRenderer>().color += Color.black * Time.deltaTime;
            if (transform.childCount > 0)
            {
                transform.GetChild(0).GetComponent<TMP_Text>().color += Color.black * Time.deltaTime;
            }
            if (GetComponent<SpriteRenderer>().color.a >= 1)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        isFade = false;
    }

    public void GetItem()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        isGet = true;
    }
}
