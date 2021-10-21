using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
