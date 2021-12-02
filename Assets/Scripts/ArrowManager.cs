using System.Collections.Generic;
using UnityEngine;

public enum ArrowCreateDirection
{
    Left,
    Right,
    Up,
    Down
} //화살표가 생성되는 위치

public class ArrowManager : Singleton<ArrowManager>
{
    public GameObject arrowPrefab;

    [SerializeField] private List<Arrow> arrows;

    private void Awake()
    {
        arrows = new List<Arrow>();
        CheckExistInstanceAndDestroy(this);
    }

    public void CreateArrowObj(Vector3 pos, ArrowCreateDirection direction, Transform parent = null)
    {
        var arrow = Instantiate(arrowPrefab, pos, GetRotate(direction)).GetComponent<Arrow>();
        arrows.Add(arrow);
        if (parent != null)
        {
            arrow.transform.parent = parent;
        }
    }

    private Quaternion GetRotate(ArrowCreateDirection direction)
    {
        var quaternion = Quaternion.identity;
        switch (direction)
        {
            case ArrowCreateDirection.Left:
                quaternion.eulerAngles = new Vector3(0, 0, 270);
                break;
            case ArrowCreateDirection.Right:
                quaternion.eulerAngles = new Vector3(0, 0, 90);
                break;
            case ArrowCreateDirection.Up:
                quaternion.eulerAngles = new Vector3(0, 0, 180);
                break;
            case ArrowCreateDirection.Down:
                quaternion.eulerAngles = new Vector3(0, 0, 0);
                break;
            default:
                quaternion.eulerAngles = new Vector3(0, 0, 0);
                break;
        }

        return quaternion;
    }

    public void DestroyAllArrow()
    {
        var arrowCnt = arrows.Count;
        for (var i = 0; i < arrowCnt; i++)
        {
            if (arrows[0] == null || arrows[0].isActiveAndEnabled == false)
            {
                arrows.RemoveAt(0);
                i++;
            }

            var arrow = arrows[0];
            arrows.RemoveAt(0);
            arrow.ArrowDestroy();
        }
    }
}
