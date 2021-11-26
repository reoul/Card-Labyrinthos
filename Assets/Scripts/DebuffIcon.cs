using UnityEngine;

public class DebuffIcon : MonoBehaviour
{
    bool onDebuffIcon;   //마우스가 필드 위에 있는지

    void OnMouseEnter()
    {
        this.onDebuffIcon = true;
    }
    void OnMouseExit()
    {
        this.onDebuffIcon = false;
    }

    private void OnMouseUp()
    {
        if (this.onDebuffIcon)
        {
            this.transform.parent.GetComponent<DebuffBar>().Open();
        }
    }
}
