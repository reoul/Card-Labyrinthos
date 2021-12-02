using UnityEngine;

public class UICloseButton : MouseInteractionObject
{
    private void OnMouseUp()
    {
        if (!OnMouse)
        {
            return;
        }

        GameManager.Inst.CloseAllUI();
    }
}
