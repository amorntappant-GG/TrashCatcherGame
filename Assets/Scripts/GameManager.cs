using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text overloadText;
    [SerializeField] private Slider overloadSlider;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    [Header("You Win")]
    [SerializeField] private GameObject winPanel;

    [Header("Game Settings")]
    [SerializeField] private int winScore = 1000;
    [SerializeField] private int maxCombo = 3;

    [Header("Overload Settings")]
    [SerializeField] private float maxOverload = 100f;
    [SerializeField] private float overloadIncrease = 10f;
    
    [Header("Overload Visuals")]
    [SerializeField] private GameObject[] overloadTrashImages; 

    private int score;
    private int combo;
    private float overload;

    private bool gameOver;
    private bool hasWon;

    public bool IsGameOver => gameOver || hasWon;

    private void Start()
    {
        score = 0;
        combo = 0;
        overload = 0f;
        gameOver = false;
        hasWon = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (overloadSlider != null)
        {
            overloadSlider.minValue = 0f;
            overloadSlider.maxValue = maxOverload;
            overloadSlider.value = overload;
        }

        UpdateUI();
        
        UpdateOverloadVisuals(); 
    }

    public void CollectWaste()
    {
        if (gameOver || hasWon)
            return;

        combo = Mathf.Min(combo + 1, maxCombo);

        score += 10 * combo;

        if (score >= winScore)
        {
            score = winScore;
            UpdateUI();
            TriggerWin();
            return;
        }

        UpdateUI();
    }

    public void AddWaste()
    {
        CollectWaste();
    }

    public void AddOverload()
    {
        if (gameOver || hasWon)
            return;

        overload += overloadIncrease;
        overload = Mathf.Clamp(overload, 0f, maxOverload);

        UpdateUI();
        
        UpdateOverloadVisuals();

        if (overload >= maxOverload)
        {
            TriggerGameOver();
        }
    }

    private void UpdateOverloadVisuals()
    {
        if (overloadTrashImages == null || overloadTrashImages.Length == 0) return;
        
        int imagesToShow = Mathf.FloorToInt(overload / 10f);

        for (int i = 0; i < overloadTrashImages.Length; i++)
        {
            if (overloadTrashImages[i] != null)
            {
                overloadTrashImages[i].SetActive(i < imagesToShow);
            }
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"SCORE {score} / {winScore}";

        if (comboText != null)
            comboText.text = $"COMBO X{combo}";

        if (overloadText != null)
            overloadText.text = $"OVERLOAD {overload:0}%";

        if (overloadSlider != null)
            overloadSlider.value = overload;
    }

    private void TriggerGameOver()
    {
        if (gameOver)
            return;

        gameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private void TriggerWin()
    {
        if (hasWon)
            return;

        hasWon = true;

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private void OnDestroy()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartGame);
    }
}