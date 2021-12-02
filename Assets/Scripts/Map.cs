using UnityEngine;

public class Map : MouseInteractionObject
{
    public static Map Inst;
    private bool isMoveCamera;
    private Vector3 lastMousePos;

    private void Awake()
    {
        Inst = this;
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();
        isMoveCamera = false;
    }

    private void Update()
    {
        if (!OnMouse)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isMoveCamera = true;
            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMoveCamera = false;
            lastMousePos = Vector3.zero;
        }

        if (isMoveCamera)
        {
            var movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePos;
            if (movePos == Vector3.zero)
            {
                return;
            }

            transform.parent.position += movePos;
            movePos = Vector3.zero;
            if (transform.parent.position.x > 30)
            {
                float x = transform.parent.position.x - 30;
                movePos.x -= x;
            }

            if (transform.parent.position.x < -30)
            {
                float x = transform.parent.position.x + 30;
                movePos.x -= x;
            }

            if (transform.parent.position.y > 25)
            {
                float y = transform.parent.position.y - 25;
                movePos.y -= y;
            }

            if (transform.parent.position.y < -25)
            {
                float y = transform.parent.position.y + 25;
                movePos.y -= y;
            }

            transform.parent.position += movePos;
            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void MoveMap(Vector3 pos)
    {
        transform.parent.position += pos;
        if (transform.parent.position.y < -15)
        {
            var position = transform.parent.position;
            position -= new Vector3(position.x, -15, position.z);
            transform.parent.position = position;
        }
    }
}
