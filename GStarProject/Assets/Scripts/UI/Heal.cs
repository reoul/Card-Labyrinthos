using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
   // public HpBar hpbar();
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("휴식방에 입장했습니다!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        Debug.Log("체력이 40회복 되었습니다!");
      //  hpbar.heal(40);
    }
}
