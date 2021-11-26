using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Inst;
    public bool onMap;
    private bool isMoveCamera;
    private Vector3 lastMousePos;

    private void Awake()
    {
        Inst = this;
    }

    private void OnMouseEnter()
    {
        onMap = true;
    }
    private void OnMouseExit()
    {
        onMap = false;
        isMoveCamera = false;
    }
    private void Update()
    {
        if (onMap)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMoveCamera = true;
                lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //lastMousePos = Input.mousePosition * 0.01f;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isMoveCamera = false;
                lastMousePos = Vector3.zero;
            }
            if (isMoveCamera)
            {
                Vector3 movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePos;
                if (movePos != Vector3.zero)
                {
                    transform.parent.position += movePos;
                    movePos = Vector3.zero;
                    if (transform.parent.position.x > 30)
                    {
                        float _x = transform.parent.position.x - 30;
                        movePos.x -= _x;
                    }
                    if (transform.parent.position.x < -30)
                    {
                        float _x = transform.parent.position.x + 30;
                        movePos.x -= _x;
                    }
                    if (transform.parent.position.y > 25)
                    {
                        float _y = transform.parent.position.y - 25;
                        movePos.y -= _y;
                    }
                    if (transform.parent.position.y < -25)
                    {
                        float _y = transform.parent.position.y + 25;
                        movePos.y -= _y;
                    }

                    transform.parent.position += movePos;
                    lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }
    }

    public void MoveMap(Vector3 pos)
    {
        transform.parent.position += pos;
        if (transform.parent.position.y < -15)
        {
            transform.parent.position -= new Vector3(transform.parent.position.x, -15, transform.parent.position.z);
        }
    }
}
