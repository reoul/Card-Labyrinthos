using UnityEngine;

public class DebuffIcon : MouseInteractionObject
{
    private void OnMouseUp()
    {
        if (OnMouse)
        {
            transform.parent.GetComponent<DebuffBar>().Open();
        }
    }
}
