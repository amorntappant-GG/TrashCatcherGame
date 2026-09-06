using UnityEngine;
using UnityEngine.UI;

public class TrashItem : MonoBehaviour
{
    [Header("Falling Settings")]
    [SerializeField] private float fallSpeed = 250f;

    private RectTransform rectTransform;
    private Button button;
    private GameManager gameManager;
    private float destroyY;
    private bool collected;

    public void Initialize(
        GameManager manager,
        float bottomPosition
    )
    {
        gameManager = manager;
        destroyY = bottomPosition;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(CollectTrash);
        }
    }

    private void Update()
    {
        if (gameManager != null && gameManager.IsGameOver)
        {
            return;
        }

        rectTransform.anchoredPosition +=
            Vector2.down * fallSpeed * Time.deltaTime;

        if (rectTransform.anchoredPosition.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }

    private void CollectTrash()
    {
        if (collected ||
            gameManager == null ||
            gameManager.IsGameOver)
        {
            return;
        }

        collected = true;
        gameManager.AddWaste();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(CollectTrash);
        }
    }
}