using System.Collections;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RectTransform trashContainer;
    [SerializeField] private RectTransform trashBin;
    [SerializeField] private TrashItem[] trashPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Position")]
    [SerializeField] private float spawnY = 600f;
    [SerializeField] private float floorY = -500f;
    [SerializeField] private float minX = -400f;
    [SerializeField] private float maxX = 400f;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (gameManager != null &&
                !gameManager.IsGameOver)
            {
                SpawnTrash();
            }
        }
    }

    private void SpawnTrash()
    {
        if (trashContainer == null) return;

        if (trashPrefabs == null ||
            trashPrefabs.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(
            0,
            trashPrefabs.Length
        );

        TrashItem newTrash = Instantiate(
            trashPrefabs[randomIndex],
            trashContainer
        );

        RectTransform trashRect =
            newTrash.GetComponent<RectTransform>();

        // สุ่มตำแหน่งซ้าย-ขวา
        float randomX = Random.Range(
            minX,
            maxX
        );

        // เริ่มจากด้านบน
        trashRect.anchoredPosition = new Vector2(
            randomX,
            spawnY
        );

        // ส่ง GameManager + ถังขยะ + ตำแหน่งพื้น
        newTrash.Initialize(
            gameManager,
            trashBin,
            floorY
        );
    }
}