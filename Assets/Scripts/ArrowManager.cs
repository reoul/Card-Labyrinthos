using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowCreateDirection { LEFT, RIGHT, UP, DOWN }        //화살표가 생성되는 위치

public class ArrowManager : MonoBehaviour
{
    public static ArrowManager Inst = null;
    public GameObject arrowPrefab;

    [SerializeField] List<Arrow> arrows;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
            arrows = new List<Arrow>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void DestoryAllArrow()
    {
        int arrowCnt = arrows.Count;
        for (int i = 0; i < arrowCnt; i++)
        {
            if (arrows[0] == null || arrows[0].isActiveAndEnabled == false)
            {
                Debug.Log("잃어버린 객체 삭제");
                arrows.RemoveAt(0);
                i++;
            }
            Debug.Log(arrows.Count);
            Debug.Log("삭제");
            var obj = arrows[0];
            arrows.RemoveAt(0);
            obj.ArrowDestory();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            CreateArrowObj(Vector3.zero, ArrowCreateDirection.UP);
    }

    public void CreateArrowObj(Vector3 pos, ArrowCreateDirection direction, Transform parent = null)
    {
        Arrow arrow = GameObject.Instantiate(arrowPrefab, pos, GetRotate(direction)).GetComponent<Arrow>();
        arrows.Add(arrow);
        if (parent != null)
        {
            arrow.transform.parent = parent;
        }
    }

    Quaternion GetRotate(ArrowCreateDirection direction)
    {
        Quaternion quaternion = Quaternion.identity;
        switch (direction)
        {
            case ArrowCreateDirection.LEFT:
                quaternion.eulerAngles = new Vector3(0, 0, 270);
                break;
            case ArrowCreateDirection.RIGHT:
                quaternion.eulerAngles = new Vector3(0, 0, 90);
                break;
            case ArrowCreateDirection.UP:
                quaternion.eulerAngles = new Vector3(0, 0, 180);
                break;
            case ArrowCreateDirection.DOWN:
                quaternion.eulerAngles = new Vector3(0, 0, 0);
                break;
            default:
                quaternion.eulerAngles = new Vector3(0, 0, 0);
                break;
        }
        return quaternion;
    }
}
