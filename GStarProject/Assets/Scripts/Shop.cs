using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{

    public GameObject shop;
    public Button closeShop;

    public void RayShop()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -0.5f;
        //Debug.DrawRay(mousePos, transform.forward, Color.red, 0.5f);
        RaycastHit2D hit2D = Physics2D.Raycast(mousePos, transform.forward, 10);    //레이 히트값 가져옴
        if (hit2D.collider != null)
        {
            if (hit2D.collider.CompareTag("Store"))
            {
                OpenShop(true);
            }
        }
    }


            public void OpenShop(bool isOpen)
            {
                shop.SetActive(isOpen);
            }

            public void CloseShop()
            {
                shop.SetActive(false);
            }

            // Start is called before the first frame update
            void Start()
    {
        closeShop.onClick.AddListener(CloseShop);
    }

    // Update is called once per frame
    void Update()
    {
                if (Input.GetMouseButtonUp(0))
                {
                    RayShop();
                }
            }
}
