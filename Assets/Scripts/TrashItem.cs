using UnityEngine;
using UnityEngine.EventSystems;

public class TrashItem : MonoBehaviour, IPointerEnterHandler
{
    [Header("Settings")]
    [SerializeField] private float fallSpeed = 10000f;
    [SerializeField] private float collectSpeed = 1000f;

    private RectTransform rectTransform;
    private GameManager gameManager;
    private RectTransform trashBin;

    private float floorY;

    private bool isOnFloor;
    private bool isCollecting;
    private bool collected;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
        if (gameManager == null)
            return;

        if (gameManager.IsGameOver)
            return;

        // =====================
        // กำลังบินไปถังขยะ
        // =====================
        if (isCollecting)
        {
            MoveToTrashBin();
            return;
        }

        // =====================
        // ตกลงพื้น
        // =====================
        if (!isOnFloor)
        {
            Vector2 position = rectTransform.anchoredPosition;

            position.y -= fallSpeed * Time.deltaTime;

            rectTransform.anchoredPosition = position;

            if (position.y <= floorY)
            {
                position.y = floorY;
                rectTransform.anchoredPosition = position;

                isOnFloor = true;

                // ขยะตกถึงพื้น
                gameManager.AddOverload();
            }
        }
    }

    // =====================
    // เมาส์แตะขยะ
    // =====================
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (collected)
            return;

        if (gameManager == null)
            return;

        if (gameManager.IsGameOver)
            return;

        if (trashBin == null)
            return;

        collected = true;
        isCollecting = true;
    }

    // =====================
    // บินไปที่ถังขยะ
    // =====================
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

        // ถึงถังแล้ว
        if (Vector2.Distance(
                rectTransform.anchoredPosition,
                targetPosition
            ) < 5f)
        {
            gameManager.CollectWaste();

            Destroy(gameObject);
        }
    }
}