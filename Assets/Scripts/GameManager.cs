using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text overloadText;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private Slider overloadSlider;
    [SerializeField] private Image overloadFill;
    [SerializeField] private Button purifyButton;

    [Header("Game Settings")]
    [SerializeField] private int winScore = 200;

    [Header("Overload Colors")]
    [SerializeField] private Color safeColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;

    private int score;
    private int combo;
    private float overload = 20f;

    private bool tutorialActive = true;
    private bool wastePressed;
    private bool purifyPressed;
    private bool gameOver;

    private Coroutine tutorialCoroutine;

    public bool IsGameOver => gameOver;

    private void Start()
    {
        if (purifyButton != null)
        {
            purifyButton.onClick.AddListener(Purify);
        }

        if (overloadSlider != null)
        {
            overloadSlider.minValue = 0f;
            overloadSlider.maxValue = 100f;
        }

        UpdateUI();
        tutorialCoroutine = StartCoroutine(OverloadTutorial());
    }

    private void Update()
    {
        if (!gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            Purify();
        }
    }

    public void AddWaste()
    {
        if (gameOver) return;

        combo++;
        score += 10 * combo;
        overload = Mathf.Clamp(overload + 15f, 0f, 100f);
        wastePressed = true;

        if (tutorialActive && tutorialText != null)
            tutorialText.text = "OVERLOAD เพิ่มขึ้น! รีบใช้ PURIFY";

        if (score >= winScore)
        {
            score = winScore;
            UpdateUI();
            TriggerWin();
            return;
        }

        UpdateUI();
        if (overload >= 100f) TriggerOverload();
    }

    public void Purify()
    {
        if (gameOver) return;

        overload = Mathf.Clamp(overload - 30f, 0f, 100f);
        combo = 0;
        purifyPressed = true;

        if (tutorialActive && tutorialText != null)
            tutorialText.text = "เยี่ยม! PURIFY ช่วยลด OVERLOAD";

        UpdateUI();
    }

    // --- (โค้ดส่วนที่เหลือเหมือนเดิมจนจบ) ---
    private IEnumerator OverloadTutorial()
    {
        tutorialActive = true;
        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(true);
            tutorialText.text = "อย่าให้ OVERLOAD เต็ม 100%!";
        }
        yield return new WaitForSeconds(2f);
        tutorialActive = false;
        if (tutorialText != null) tutorialText.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"SCORE {score} / {winScore}";
        if (comboText != null) comboText.text = $"COMBO X{combo}";
        if (overloadText != null) overloadText.text = $"OVERLOAD\n{Mathf.RoundToInt(overload)}%";
        if (overloadSlider != null) overloadSlider.value = overload;
        UpdateOverloadColor();
    }

    private void UpdateOverloadColor()
    {
        if (overloadFill == null) return;
        if (overload < 50f) overloadFill.color = safeColor;
        else if (overload < 80f) overloadFill.color = warningColor;
        else overloadFill.color = dangerColor;
    }

    private void TriggerWin()
    {
        gameOver = true;
        if (purifyButton != null) purifyButton.interactable = false;
        if (tutorialText != null) tutorialText.text = "YOU WIN!";
    }

    private void TriggerOverload()
    {
        gameOver = true;
        if (purifyButton != null) purifyButton.interactable = false;
        if (tutorialText != null) tutorialText.text = "OVERLOAD! 100%";
    }

    private void OnDestroy()
    {
        if (purifyButton != null) purifyButton.onClick.RemoveListener(Purify);
    }
}