using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public bool onMap = false;
    bool isMoveCamera;
    Vector3 lastMousePos;

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
                Debug.Log(movePos);
                if (movePos != Vector3.zero)
                {
                    this.transform.parent.position += movePos;
                    movePos = Vector3.zero;
                    if (this.transform.parent.position.x > 25.59731f)
                    {
                        float _x = this.transform.parent.position.x - 25.59731f;
                        movePos.x -= _x;
                    }
                    if (this.transform.parent.position.x < -20.29902f)
                    {
                        float _x = this.transform.parent.position.x + 20.29902f;
                        movePos.x -= _x;
                    }
                    if (this.transform.parent.position.y > 19.7925f)
                    {
                        float _y = this.transform.parent.position.y - 19.7925f;
                        movePos.y -= _y;
                    }
                    if (this.transform.parent.position.y < -19.6774f)
                    {
                        float _y = this.transform.parent.position.y + 19.6774f;
                        movePos.y -= _y;
                    }
                    this.transform.parent.position += movePos;
                    lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }
    }

    public void MoveUI(Vector3 pos)
    {
        Camera.main.transform.position -= pos;
        TopBar.Inst.transform.position -= pos;
        RewardManager.Inst.transform.position -= pos;
        BagManager.Inst.transform.position -= pos;
        FadeManager.Inst.transform.position -= pos;
        SkillManager.Inst.transform.position -= pos;
    }
}
