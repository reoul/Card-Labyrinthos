using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    public static ResetManager Inst = null;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ResetGame()
    {
        GameObject[] managers = GameObject.FindGameObjectsWithTag("Manager");
        for (int i = 0; i < managers.Length; i++)
        {
            Destroy(managers[i]);
        }
        SceneManager.LoadScene("Intro");
    }
}
