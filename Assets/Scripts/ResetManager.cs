using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : Singleton<ResetManager>
{
    private void Awake()
    {
        CheckExistInstanceAndDestroy(this);
    }

    public void ResetGame()
    {
        var managerObjs = GameObject.FindGameObjectsWithTag("Manager");

        foreach (GameObject managerObj in managerObjs)
        {
            var singleton = managerObj.GetComponent<ISingleton>();
            singleton?.DestroySingleton();
        }

        SceneManager.LoadScene("Intro");
    }
}
