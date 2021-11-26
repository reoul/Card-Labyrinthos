using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    public static ResetManager Inst;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
