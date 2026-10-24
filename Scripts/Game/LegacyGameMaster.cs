using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Initial version of the game master, that counts duration and score, instead of tracking levels.
/// </summary>
public class LegacyGameMaster : MonoBehaviour
{
    public static LegacyGameMaster Instance { get; private set; }
    
    // The list of timestamps at which the tier changes, and the score starts updating faster.
    [SerializeField] int[] scoreTiers;
    [SerializeField] UIDocument gameUI;
    Label scoreLabel;
    GroupBox gameOverMenu;
    bool isGameOver;
    float gameStartTime;
    float elapsedTime;
    float lastScoreUpdateTime;
    float scoreUpdateInterval;
    int currentTier;
    int currentScore;

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

        scoreLabel = gameUI.rootVisualElement.Q<Label>("ScoreLabel");
        gameOverMenu = gameUI.rootVisualElement.Q<GroupBox>("GameOverMenu");
    }

    void Start()
    {
        StartGame();
        gameOverMenu.style.display = DisplayStyle.None;
    }

    void Update()
    {
        if (!isGameOver) ManageScore();
    }

    public void StartGame()
    {
        isGameOver = false;
        gameStartTime = Time.time;
        elapsedTime = 0f;
        scoreUpdateInterval = 2.2f;
        currentTier = 0;
        currentScore = 0;
    }

    void ManageScore()
    {
        elapsedTime = Time.time - gameStartTime;
        float timeSinceLastUpdate = Time.time - lastScoreUpdateTime;
        if (timeSinceLastUpdate > scoreUpdateInterval)
        {
            currentScore++;
            lastScoreUpdateTime = Time.time;
            if (elapsedTime > scoreTiers[currentTier])
            {
                currentTier++;
                scoreUpdateInterval = 1 / currentTier;
            }
        }
        scoreLabel.text = currentScore.ToString();
        DebuggerConsole.WriteToBox(2, elapsedTime.ToString("F4"));
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverMenu.style.display = DisplayStyle.Flex;
    }
}
