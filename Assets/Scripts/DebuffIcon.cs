using UnityEngine;

public class DebuffIcon : MonoBehaviour
{
    private bool _onDebuffIcon;   //마우스가 필드 위에 있는지

    private void OnMouseEnter()
    {
        _onDebuffIcon = true;
    }

    private void OnMouseExit()
    {
        _onDebuffIcon = false;
    }

    private void OnMouseUp()
    {
        if (_onDebuffIcon)
        {
            transform.parent.GetComponent<DebuffBar>().Open();
        }
    }
}
