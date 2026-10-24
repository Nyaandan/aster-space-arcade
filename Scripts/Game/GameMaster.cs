using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayableDirector))]
public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }

    PlayableDirector director;
    string level;

    [SerializeField] UIDocument gameUI;

    
    bool isGameOver;
    int currentPhaseIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        director = GetComponent<PlayableDirector>();
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        
    }

    public void StartGame()
    {
        isGameOver = false;
        GameUI.Instance.ToggleGameOverMenu(false);
        currentPhaseIndex = 0;

        // Additional logic to start the game with the provided level timeline
        string level = LevelManager.Instance.currentLevel;
        director.playableAsset = Resources.Load<PlayableAsset>("Levels/" + level);
        director.Play();
    }

    public void GameOver(bool win)
    {
        if (isGameOver) return;
        isGameOver = true;

        director.Stop();
        GameUI.Instance.ToggleGameOverMenu(true, win ? "LEVEL CLEARED!" : "GAME OVER...");
    }

    public void StartNextLevel()
    {
        string level = LevelManager.Instance.nextLevel;
        StartCoroutine(LoadLevel(level));
    }

    public void RestartLevel()
    {
        string level = LevelManager.Instance.currentLevel;
        StartCoroutine(LoadLevel(level));
    }

    public IEnumerator LoadLevel(string level)
    {
        while (!LevelManager.Instance.LoadLevel(level).isDone) yield return null;
    }

    public void GoToMainMenu()
    {
        print("MAIN MENU!");
    }
}
