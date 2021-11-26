using DG.Tweening;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Ease ease;

    public Transform waypoint1;
    public Transform waypoint2;
    public Transform waypoint3;

    private Vector3[] waypoints;

    void Start()
    {
        //DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        this.waypoints = new Vector3[3];
        this.waypoints.SetValue(this.waypoint1.position, 0);
        this.waypoints.SetValue(this.waypoint2.position, 1);
        this.waypoints.SetValue(this.waypoint3.position, 2);

        this.transform.DOPath(this.waypoints, 0.7f, PathType.CatmullRom).SetLookAt(new Vector3(0, 0, 0)).SetEase(this.ease).SetLoops(-1, LoopType.Yoyo);
    }

}
