using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneSwitcher : MonoBehaviour
{
    private static SceneSwitcher instance;
    private Stack<string> sceneHistory = new Stack<string>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            EventSystem existingEventSystem = FindObjectOfType<EventSystem>();
            if (existingEventSystem != null && existingEventSystem.gameObject != this.gameObject)
            {
                DontDestroyOnLoad(existingEventSystem.gameObject);
            }
        }
        else
        {
            instance.sceneHistory = this.sceneHistory;
            Destroy(gameObject);
        }
    }

    public void SwitchScene(string sceneName)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        sceneHistory.Push(currentSceneName);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.SetActiveScene(scene);

        if (sceneHistory.Count > 0)
        {
            string previousScene = sceneHistory.Peek();
            SceneManager.UnloadSceneAsync(previousScene);
        }
    }

    public void LoadPreviousScene() // 回到上一個場景
    {
        if (sceneHistory.Count > 0)
        {
            string previousScene = sceneHistory.Pop();
            SceneManager.LoadScene(previousScene, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }
}
