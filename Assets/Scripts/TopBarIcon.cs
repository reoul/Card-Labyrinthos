using UnityEngine;

public class TopBarIcon : MonoBehaviour
{
    public TOPBAR_TYPE type;
    bool onTopBarIcon;   //마우스가 필드 위에 있는지
    bool _isLock;
    public bool isLock
    {
        get { return this._isLock; }
        private set { this._isLock = value; }
    }

    void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        this.onTopBarIcon = true;
        switch (this.type)
        {
            case TOPBAR_TYPE.BAG:
            case TOPBAR_TYPE.SETTING:
            case TOPBAR_TYPE.SKILL:
                TopBar.Inst.OnMouseEnterIcon(this.type);
                break;
        }
    }
    void OnMouseExit()
    {
        this.onTopBarIcon = false;
        switch (this.type)
        {
            case TOPBAR_TYPE.BAG:
            case TOPBAR_TYPE.SETTING:
            case TOPBAR_TYPE.SKILL:
                TopBar.Inst.OnMouseExitIcon(this.type);
                break;
        }
    }

    private void OnMouseUp()
    {
        if (this.onTopBarIcon)
        {
            TopBar.Inst.Open(this);
        }
    }

    public void Lock()
    {
        this.isLock = true;
    }
    public void UnLock()
    {
        this.isLock = false;
    }

}
