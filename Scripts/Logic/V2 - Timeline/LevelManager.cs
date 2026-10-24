using System.Collections.Specialized;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public string currentLevel;
    public string nextLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AsyncOperation LoadLevel(string name)
    {
        currentLevel = name;
        return SceneManager.LoadSceneAsync("Game Scene", LoadSceneMode.Single);
    }

}
