using DG.Tweening;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Ease ease;

    public Transform waypoint1;
    public Transform waypoint2;
    public Transform waypoint3;

    private Vector3[] waypoints;

    private void Start()
    {
        //DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        waypoints = new Vector3[3];
        waypoints.SetValue(waypoint1.position, 0);
        waypoints.SetValue(waypoint2.position, 1);
        waypoints.SetValue(waypoint3.position, 2);

        transform.DOPath(waypoints, 0.7f, PathType.CatmullRom).SetLookAt(new Vector3(0, 0, 0)).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

}
