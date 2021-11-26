using System.Collections.Generic;
using UnityEngine;

public enum ArrowCreateDirection
{
    LEFT,
    RIGHT,
    UP,
    DOWN
} //화살표가 생성되는 위치

public class ArrowManager : MonoBehaviour
{
    public static ArrowManager Inst;
    public GameObject arrowPrefab;

    [SerializeField] List<Arrow> arrows;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
            this.arrows = new List<Arrow>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void DestoryAllArrow()
    {
        int arrowCnt = this.arrows.Count;
        for (int i = 0; i < arrowCnt; i++)
        {
            if (this.arrows[0] == null || this.arrows[0].isActiveAndEnabled == false)
            {
                this.arrows.RemoveAt(0);
                i++;
            }

            var arrow = this.arrows[0];
            this.arrows.RemoveAt(0);
            arrow.ArrowDestory();
        }
    }

    public void CreateArrowObj(Vector3 pos, ArrowCreateDirection direction, Transform parent = null)
    {
        var arrow = Instantiate(this.arrowPrefab, pos, this.GetRotate(direction)).GetComponent<Arrow>();
        this.arrows.Add(arrow);
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
