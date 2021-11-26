using UnityEngine;

public class UICloseButton : MonoBehaviour
{
    bool isOnButton;

    private void OnMouseEnter()
    {
        this.isOnButton = true;
    }

    private void OnMouseExit()
    {
        this.isOnButton = false;
    }

    private void OnMouseUp()
    {
        if (this.isOnButton)
            GameManager.Inst.CloseAllUI();
    }
}
