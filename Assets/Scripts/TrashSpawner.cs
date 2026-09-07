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
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private int trashPerSpawn = 5;

    [Header("Position")]
    [SerializeField] private float spawnY = 600f;
    [SerializeField] private float floorY = -500f;
    [SerializeField] private float minX = -400f;
    [SerializeField] private float maxX = 400f;

    private IEnumerator Start()
    {
        while (true)
        {
            if (gameManager != null &&
                !gameManager.IsGameOver)
            {
                // Spawn ขยะหลายชิ้นพร้อมกัน
                SpawnTrashGroup();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnTrashGroup()
    {
        for (int i = 0; i < trashPerSpawn; i++)
        {
            SpawnTrash();
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

        newTrash.Initialize(
            gameManager,
            trashBin,
            floorY
        );
    }
}