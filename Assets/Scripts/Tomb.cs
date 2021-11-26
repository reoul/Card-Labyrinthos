using System.Collections;
using UnityEngine;

public class Tomb : MonoBehaviour
{
    bool onIcon;   //마우스가 필드 위에 있는지
    bool isGet;
    bool isFade = true;

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
            this.StartCoroutine(TutorialManager.Inst.GetCardCoroutine());
            this.isFade = true;
        }
    }

    public IEnumerator SetLook()
    {
        while (true)
        {
            this.GetComponent<SpriteRenderer>().color += Color.black * Time.deltaTime;
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
