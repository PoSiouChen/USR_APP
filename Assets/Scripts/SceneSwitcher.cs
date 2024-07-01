using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagers : MonoBehaviour
{
    public static SceneManagers Singleton;
    private static List<string> LastScene = new List<string>();

    void Awake()
    {
        Singleton = this;
    }

    public void LoadScene(string SceneName)
    {
        LastScene.Add(SceneManager.GetActiveScene().name); // 保存目前場景的名字
        SceneManager.LoadScene(SceneName); //載新場景
    }

    public void LoadLastScene()
    {
        if(LastScene.Count > 0){
            SceneManager.LoadScene(LastScene[LastScene.Count - 1]);
            LastScene.RemoveAt(LastScene.Count - 1);
        } else{
            Debug.Log("已是最初場景，無法退回");
        }
    }

}
