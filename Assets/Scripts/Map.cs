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
                lastMousePos = Input.mousePosition * 0.01f;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isMoveCamera = false;
                lastMousePos = Vector3.zero;
            }
            if (isMoveCamera)
            {
                Vector3 movePos = Input.mousePosition * 0.01f - lastMousePos;
                if (movePos != Vector3.zero)
                {
                    if (Camera.main.transform.position.x - movePos.x < -23.57998f)
                    {
                        float _x = Camera.main.transform.position.x - movePos.x + 23.57998f;
                        movePos.x += _x;
                    }
                    if (Camera.main.transform.position.x - movePos.x > 20.03997f)
                    {
                        float _x = Camera.main.transform.position.x - movePos.x - 20.03997f;
                        movePos.x += _x;
                    }
                    if (Camera.main.transform.position.y - movePos.y > 18.55997f)
                    {
                        float _y = Camera.main.transform.position.y - movePos.y - 18.55997f;
                        movePos.y += _y;
                    }
                    if (Camera.main.transform.position.y - movePos.y < -18.05999f)
                    {
                        float _y = Camera.main.transform.position.y - movePos.y + 18.05999f;
                        movePos.y += _y;
                    }
                    MoveUI(movePos);
                    lastMousePos = Input.mousePosition * 0.01f;
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
