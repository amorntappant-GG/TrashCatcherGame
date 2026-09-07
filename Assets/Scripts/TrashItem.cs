using UnityEngine;
using UnityEngine.UI;

public class TrashItem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fallSpeed = 250f;
    [SerializeField] private float collectSpeed = 800f;

    private RectTransform rectTransform;
    private Button button;
    private GameManager gameManager;
    private RectTransform trashBin;

    private float floorY;

    private bool isOnFloor;
    private bool isCollecting;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(CollectTrash);
        }
    }

    public void Initialize(
        GameManager manager,
        RectTransform bin,
        float floorPosition
    )
    {
        gameManager = manager;
        trashBin = bin;
        floorY = floorPosition;
    }

    private void Update()
    {
        if (gameManager != null && gameManager.IsGameOver)
            return;

        // กำลังบินเข้าถัง
        if (isCollecting)
        {
            MoveToTrashBin();
            return;
        }

        // ตกลงพื้น
        if (!isOnFloor)
        {
            rectTransform.anchoredPosition +=
                Vector2.down * fallSpeed * Time.deltaTime;

            if (rectTransform.anchoredPosition.y <= floorY)
            {
                Vector2 position = rectTransform.anchoredPosition;

                position.y = floorY;

                rectTransform.anchoredPosition = position;

                isOnFloor = true;
            }
        }
    }

    private void CollectTrash()
    {
        if (isCollecting)
            return;

        if (gameManager == null)
            return;

        if (gameManager.IsGameOver)
            return;

        if (trashBin == null)
            return;

        isCollecting = true;

        // ปิดการกดซ้ำ
        if (button != null)
        {
            button.interactable = false;
        }
    }

    private void MoveToTrashBin()
    {
        Vector2 targetPosition =
            rectTransform.parent.InverseTransformPoint(
                trashBin.position
            );

        rectTransform.anchoredPosition =
            Vector2.MoveTowards(
                rectTransform.anchoredPosition,
                targetPosition,
                collectSpeed * Time.deltaTime
            );

        float distance = Vector2.Distance(
            rectTransform.anchoredPosition,
            targetPosition
        );

        if (distance < 5f)
        {
            gameManager.AddWaste();

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(CollectTrash);
        }
    }
}