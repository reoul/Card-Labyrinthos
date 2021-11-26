using UnityEngine;

public class UICloseButton : MonoBehaviour
{
    private bool isOnButton;

    private void OnMouseEnter()
    {
        isOnButton = true;
    }

    private void OnMouseExit()
    {
        isOnButton = false;
    }

    private void OnMouseUp()
    {
        if (isOnButton)
        {
            GameManager.Inst.CloseAllUI();
        }
    }
}
