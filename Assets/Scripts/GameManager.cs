using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text tutorialText;

    [Header("Game Settings")]
    [SerializeField] private int winScore = 500;

    private int score;
    private int combo;
    private bool gameOver;

    public bool IsGameOver => gameOver;

    private void Start()
    {
        UpdateUI();

        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(true);
            tutorialText.text = "CLICK THE TRASH!";
        }
    }

    // เก็บขยะสำเร็จ
    public void CollectWaste()
    {
        if (gameOver) return;

        combo++;
        score += 10 * combo;

        UpdateUI();

        if (score >= winScore)
        {
            score = winScore;
            UpdateUI();
            TriggerWin();
        }
    }

    // ใช้สำหรับ TrashItem เรียกเมื่อขยะถึงถัง
    public void AddWaste()
    {
        CollectWaste();
    }

    // ขยะตกถึงพื้น
    public void TrashHitFloor()
    {
        if (gameOver) return;

        TriggerGameOver();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE {score} / {winScore}";
        }

        if (comboText != null)
        {
            comboText.text = $"COMBO X{combo}";
        }
    }

    private void TriggerWin()
    {
        gameOver = true;

        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(true);
            tutorialText.text = "YOU WIN!";
        }
    }

    private void TriggerGameOver()
    {
        gameOver = true;

        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(true);
            tutorialText.text = "GAME OVER!";
        }
    }
}