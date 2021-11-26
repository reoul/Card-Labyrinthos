using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Inst;
    public bool onMap;
    bool isMoveCamera;
    Vector3 lastMousePos;

    private void Awake()
    {
        Inst = this;
    }

    private void OnMouseEnter()
    {
        this.onMap = true;
    }
    private void OnMouseExit()
    {
        this.onMap = false;
        this.isMoveCamera = false;
    }
    private void Update()
    {
        if (this.onMap)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.isMoveCamera = true;
                this.lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //lastMousePos = Input.mousePosition * 0.01f;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                this.isMoveCamera = false;
                this.lastMousePos = Vector3.zero;
            }
            if (this.isMoveCamera)
            {
                Vector3 movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.lastMousePos;
                if (movePos != Vector3.zero)
                {
                    this.transform.parent.position += movePos;
                    movePos = Vector3.zero;
                    if (this.transform.parent.position.x > 30)
                    {
                        float _x = this.transform.parent.position.x - 30;
                        movePos.x -= _x;
                    }
                    if (this.transform.parent.position.x < -30)
                    {
                        float _x = this.transform.parent.position.x + 30;
                        movePos.x -= _x;
                    }
                    if (this.transform.parent.position.y > 25)
                    {
                        float _y = this.transform.parent.position.y - 25;
                        movePos.y -= _y;
                    }
                    if (this.transform.parent.position.y < -25)
                    {
                        float _y = this.transform.parent.position.y + 25;
                        movePos.y -= _y;
                    }

                    this.transform.parent.position += movePos;
                    this.lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }
    }

    public void MoveMap(Vector3 pos)
    {
        this.transform.parent.position += pos;
        if (this.transform.parent.position.y < -15) this.transform.parent.position -= new Vector3(this.transform.parent.position.x, -15, this.transform.parent.position.z);
    }
}
